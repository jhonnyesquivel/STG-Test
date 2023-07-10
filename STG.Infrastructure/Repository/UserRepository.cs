using Dapper;
using Microsoft.Extensions.Configuration;
using STG.Core.Entities;
using STG.Core.Repository;
using System.Data.SqlClient;

namespace STG.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var query = "SELECT * FROM [STGenetics].[dbo].[User] WHERE Username = @Username";
            var parameters = new { Username = username };
            using var connection = new SqlConnection(_connectionString);
            var user = await connection.QuerySingleOrDefaultAsync<User>(query, parameters);
            return user;
        }
    }
}