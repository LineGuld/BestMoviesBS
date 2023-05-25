using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using BestMoviesBS.DataAccess;
using BestMoviesBS.Models;
using Neo4j.Driver;
using Newtonsoft.Json;

namespace BestMoviesBS.DataAccess
{
    public class UserDao : IUserDao
    {
        private string connectionString = Environment.GetEnvironmentVariable("Connectionstring");

        private User _user = new User();
        private Toplist _toplist = new Toplist();
        private bool _disposed = false;
        private readonly IDriver _driver;

        

        public UserDao()
        {
           
        }


        public async Task<User> FindUser(string userID)
        {
            User user = new User();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @$"SELECT ID, username
                                        FROM [User] WHERE ID = '{userID}'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                       
                        while (reader.Read())
                        {
                            user.Id = reader.GetString(0);
                            user.Username = reader.GetString(1);
                        }
                    }
                    reader.Close();
                }
            }
            return user;
        }

        public async Task<Toplist> GetToplist(string userId)
        {
            
            Toplist toplist = new Toplist();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @$"SELECT top(5) *
                                        FROM [Toplist] WHERE userID = '{userId}' order by ID desc";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //toplist.Id = reader.GetInt32(0);
                            toplist.UserId = reader.GetString(1);
                            toplist.TitleIds.Add(reader.GetInt32(2));
                        }
                        reader.NextResult();
                    }
                    reader.Close();
                }
            }
            return toplist;
        }

        /*public async Task SetToplist(string userId, Toplist toplist)
        {
            var query = $@"
            MATCH (u:User {{id: $id}})-->(t:Toplist)
            SET t.title1 = $tmdbid1
            SET t.title2 = $tmdbid2
            SET t.title3 = $tmdbid3
            SET t.title4 = $tmdbid4
            SET t.title5 = $tmdbid5
            RETURN t";

            await using var session = _driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));
            try
            {
                var writeResults = await session.ExecuteWriteAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new
                    {
                        tmdbid1 = toplist.TitleIds[0],
                        tmdbid2 = toplist.TitleIds[1],
                        tmdbid3 = toplist.TitleIds[2],
                        tmdbid4 = toplist.TitleIds[3],
                        tmdbid5 = toplist.TitleIds[4],
                        id = userId
                    });
                    return await result.ToListAsync();
                });

                /*foreach (var result in writeResults)
                {
                    Console.WriteLine("Titles " + _toplist.TitleIds);
                    Console.WriteLine(
                        $"Added movie {result["u.username"].As<String>()} to toplist no. {result["t.id"].As<Int32>()}");
                }#1#
            }
            catch (Neo4jException ex)
            {
                Console.WriteLine($"{query} - {ex}");
                throw;
            }
        }*/

        public async Task<Toplist> AddMovieToToplist(string userId, int tmdbId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @$"Insert into Toplist(userID, movieID) values ('{userId}', {tmdbId})";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                DataSet toplists = new DataSet();
                adapter.Fill(toplists, "Toplist");
            }
            return await GetToplist(userId);
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