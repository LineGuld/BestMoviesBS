using BestMoviesBS.Models;

namespace BestMoviesBS.DataAccess;
 using Neo4j.Driver;
public class UserDao : IUserDao
{
        private string uri = Environment.GetEnvironmentVariable("uri");
        private string user = Environment.GetEnvironmentVariable("username");
        private string password = Environment.GetEnvironmentVariable("password");
        
        private User _user = new User();
        private Toplist _toplist = new Toplist();
        private bool _disposed = false;
        private readonly IDriver _driver;

        ~UserDao() => Dispose(false);

        public UserDao()
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        /*   public async Task CreateFriendship(string person1Name, string person2Name)
         {
             // To learn more about the Cypher syntax, see https://neo4j.com/docs/cypher-manual/current/
             // The Reference Card is also a good resource for keywords https://neo4j.com/docs/cypher-refcard/current/
             var query = @"
             MERGE (p1:Person { name: $person1Name })
             MERGE (p2:Person { name: $person2Name })
             MERGE (p1)-[:KNOWS]->(p2)
             RETURN p1, p2";
     
             await using var session = _driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));
             try
             {
                 // Write transactions allow the driver to handle retries and transient error
                 var writeResults = await session.ExecuteWriteAsync(async tx =>
                 {
                     var result = await tx.RunAsync(query, new { person1Name, person2Name });
                     return await result.ToListAsync();
                 });
     
                 foreach (var result in writeResults)
                 {
                     var person1 = result["p1"].As<INode>().Properties["name"];
                     var person2 = result["p2"].As<INode>().Properties["name"];
                     Console.WriteLine($"Created friendship between: {person1}, {person2}");
                 }
             }
             // Capture any errors along with the query and data for traceability
             catch (Neo4jException ex)
             {
                 Console.WriteLine($"{query} - {ex}");
                 throw;
             }
         } */

        public async Task FindUser(string userName)
        {
            var query = @"
        MATCH (u:User)
        WHERE u.username = $username
        RETURN u.username, u.id";

            await using var session = _driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));
            try
            {
                var readResults = await session.ExecuteReadAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new {username = userName});
                    return await result.ToListAsync();
                });

                foreach (var result in readResults)
                {
                    _user.Id = result["u.id"].As<String>();
                    _user.Username = result["u.username"].As<String>();
                    Console.WriteLine(_user.Id);
                    Console.WriteLine(_user.Username);
                    Console.WriteLine(
                        $"Found user {result["u.username"].As<String>()} with id {result["u.id"].As<String>()}");
                }
            }
            // Capture any errors along with the query and data for traceability
            catch (Neo4jException ex)
            {
                Console.WriteLine($"{query} - {ex}");
                throw;
            }
        }

        public async Task<Toplist> GetToplist(string userId)
        {
            var query = $@"
        MATCH (u:User {{id: $id}})-->(t:Toplist)
        RETURN t.title1, t.title2, t.title3, t.title4, t.title5";

            await using var session = _driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));
            try
            {
                var readResults = await session.ExecuteReadAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new {id = userId});
                    return await result.ToListAsync();
                });

                foreach (var result in readResults)
                {
                    _toplist.TitleIds.SetValue(result["t.title1"].As<int>(), 0);
                    _toplist.TitleIds.SetValue(result["t.title2"].As<int>(), 1);
                    _toplist.TitleIds.SetValue(result["t.title3"].As<int>(), 2);
                    _toplist.TitleIds.SetValue(result["t.title4"].As<int>(), 3);
                    _toplist.TitleIds.SetValue(result["t.title5"].As<int>(), 4);

                    for (int i = 0; i < _toplist.TitleIds.Length; i++)
                    {
                        Console.WriteLine("Title " + _toplist.TitleIds.GetValue(i));
                    }
                }

                return _toplist;
            }
            // Capture any errors along with the query and data for traceability
            catch (Neo4jException ex)
            {
                Console.WriteLine($"{query} - {ex}");
                throw new Exception($@"ERROR: {ex.Code}, {ex.Message}");
            }
        }

        public async Task AddMovieToToplist(string userId, int tmdbId, int toplistNumber)
        {
            var query = $@"
        MATCH (u:User {{id: $id}})-->(t:Toplist)
        SET t.title{toplistNumber} = $tmdbid
        RETURN t";

            await using var session = _driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));
            try
            {
                var writeResults = await session.ExecuteWriteAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new {tmdbid = tmdbId, id = userId});
                    return await result.ToListAsync();
                });

                await GetToplist(userId);
                Toplist userToplist = _toplist;

                // få users toplist
                // skub film på pladserne toplistNumber-(toplist.count-1) én ned
                // film på index 4 gemmes ikke
                // sæt tmdbId ind på index toplistNumber, som nu er tom
                
                foreach (var result in writeResults)
                {
                    //string toplistNo = $"Title{toplistNumber}";


                    // Console.WriteLine(toplist.titleIds[toplistNumber]);

                    //  toplist.Id = result["t.id"].As<Int32>();
                    // Console.WriteLine("Toplist " + _toplist.Id);


                    //_toplist.titleIds[toplistNumber] = result[tmdbId].As<int>();
                    //toplist.titles.SetValue(result[tmdbId].As<Int64>(), toplistNumber-1);

                    // Console.WriteLine("Titles " + _toplist.titleIds);
                    Console.WriteLine(
                        $"Added movie {result["u.username"].As<String>()} to toplist no. {result["t.id"].As<Int32>()}");
                }
            }
            // Capture any errors along with the query and data for traceability
            catch (Neo4jException ex)
            {
                Console.WriteLine($"{query} - {ex}");
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _driver?.Dispose();
            }

            _disposed = true;
        }
    
}