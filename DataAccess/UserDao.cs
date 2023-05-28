﻿using System;
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
        private string uri = Environment.GetEnvironmentVariable("uri");
        private string user = Environment.GetEnvironmentVariable("user");
        private string password = Environment.GetEnvironmentVariable("password");
        private string connectionString = Environment.GetEnvironmentVariable("Connectionstring");

        private bool _disposed = false;
        private readonly IDriver _driver;

         ~UserDao() => Dispose(false);

        public UserDao()
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }


        public async Task<User> FindUser(string userID)
        {
            User user = new User();
            
            var query = @"
            MATCH (u:User)
            WHERE u.id = $id
            RETURN u.username, u.id";

            await using var session = _driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));
            try
            {
                var readResults = await session.ExecuteReadAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new {id = userID});
                    return await result.ToListAsync();
                });

                foreach (var result in readResults)
                {
                    user.Id = result["u.id"].As<String?>();
                    user.Username = result["u.username"].As<String?>();
                
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
            return user;
        }

        public async Task<User> AddUser(User user)
        {
            var query = $@"
            CREATE (u:User {{id: $id, username: $username}})-[:Has]->(t :Toplist) RETURN u, t";

            await using var session = _driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));
            try
            {
                var writeResults = await session.ExecuteWriteAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new
                    {
                        id = user.Id,
                        username = user.Username
                    });
                    return await result.ToListAsync();
                });

                return await FindUser(user.Id);
            }
            catch (Neo4jException ex)
            {
                Console.WriteLine($"{query} - {ex}");
                throw;
            }
        }

        public async Task<Toplist> GetToplist(string userId)
        {
         Toplist _toplist = new Toplist(userId);
        
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
                    _toplist.TitleIds.Insert(0,result["t.title1"].As<int?>());
                    _toplist.TitleIds.Insert(1,result["t.title2"].As<int?>());
                    _toplist.TitleIds.Insert(2,result["t.title3"].As<int?>());
                    _toplist.TitleIds.Insert(3,result["t.title4"].As<int?>());
                    _toplist.TitleIds.Insert(4,result["t.title5"].As<int?>());

                    for (int i = 0; i < _toplist.TitleIds.Count; i++)
                    {
                        Console.WriteLine("Title " + _toplist.TitleIds[i]);
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

        public async Task SetToplist(string userId, Toplist toplist)
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