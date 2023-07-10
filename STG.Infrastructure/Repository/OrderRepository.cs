using Dapper;
using Microsoft.Extensions.Configuration;
using STG.Core.Entities;
using STG.Core.Repository;
using System.Data.SqlClient;

namespace STG.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<Order> GetById(int orderId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var order = await connection.QuerySingleOrDefaultAsync<Order>("SELECT * FROM Order WHERE OrderId = @OrderId", new { OrderId = orderId });
            return order;
        }

        public async Task Create(Order order)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var insertQuery = "INSERT INTO Order (TotalAmount, Freight) VALUES (@TotalAmount, @Freight); SELECT SCOPE_IDENTITY()";
            var orderId = await connection.ExecuteScalarAsync<int>(insertQuery, order);
            order.OrderId = orderId;
        }

        public async Task<bool> IsAnimalAlreadyInOrder(int orderId, int animalId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var query = "SELECT COUNT(*) FROM OrderAnimal WHERE OrderId = @OrderId AND AnimalId = @AnimalId";
            var count = await connection.ExecuteScalarAsync<int>(query, new { OrderId = orderId, AnimalId = animalId });
            return count > 0;
        }
    }
}