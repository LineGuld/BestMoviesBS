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

        public Task<Movie> PutMovie(Movie movie)
        {
            throw new NotImplementedException();
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