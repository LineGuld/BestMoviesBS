using BestMoviesBS.Models;
using Neo4j.Driver;

namespace BestMoviesBS.DataAccess
{
    public class MovieDao : IMovieDao
    {
        private string uri = Environment.GetEnvironmentVariable("uri");
        private string user = Environment.GetEnvironmentVariable("user");
        private string password = Environment.GetEnvironmentVariable("password");

        private bool _disposed = false;
        private readonly IDriver _driver;

        ~MovieDao() => Dispose(false);

        public MovieDao()
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public async Task<Movie> GetMovie(int? tmbdid)
        {
            Movie movie = new Movie();

            var query = @"
            MATCH (m:Movie) 
            WHERE m.tmdbid = $id 
            RETURN m.tmdbid, m.title";

            await using var session = _driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));
            try
            {
                var readResults = await session.ExecuteReadAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new {id = tmbdid});
                    return await result.ToListAsync();
                });

                foreach (var result in readResults)
                {
                    movie.Tmdbid = result["m.tmdbid"].As<int>();
                    movie.Title = result["m.title"].As<String?>();
                }
            }
            // Capture any errors along with the query and data for traceability
            catch (Neo4jException ex)
            {
                Console.WriteLine($"{query} - {ex}");
                throw;
            }

            return movie;
        }

        public async Task<Movie> PutMovie(Movie movie)
        {
            var query = $@"
            CREATE (m :Movie {{tmdbid: $id, title: $title}}) 
            RETURN m.tmdbid, m.title";
            await using var session = _driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));
            try
            {
                var writeResults = await session.ExecuteWriteAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new
                    {
                        id = movie.Tmdbid,
                        title = movie.Title
                    });
                    return await result.ToListAsync();
                });
                
                foreach (var result in writeResults)
                {
                    movie.Tmdbid = result["m.tmdbid"].As<int>();
                    movie.Title = result["m.title"].As<String?>();
                }
                
                return movie;
            }
            catch (Neo4jException ex)
            {
                Console.WriteLine($"{query} - {ex}");
                throw;
            }
        }

        public async Task<Movie> SetTitle(Movie movie)
        {
            var query = $@"
            MATCH (m :Movie {{tmdbid: $id}}) 
            SET m.title = $title
            RETURN m.tmdbid, m.title";
            
            await using var session = _driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));
            try
            {
                var writeResults = await session.ExecuteWriteAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new
                    {
                        id = movie.Tmdbid,
                        title = movie.Title
                    });
                    return await result.ToListAsync();
                });
                
                foreach (var result in writeResults)
                {
                    movie.Tmdbid = result["m.tmdbid"].As<int>();
                    movie.Title = result["m.title"].As<String?>();
                }
                
                return movie;
            }
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
}