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
            using var transaction = connection.BeginTransaction();
            try
            {
                var orderId = await connection.ExecuteScalarAsync<int>(@"
                        INSERT INTO [Order] (UserId, Freight, TotalAmount)
                        VALUES (@UserId, @Freight, @TotalAmount);
                        SELECT CAST(SCOPE_IDENTITY() AS INT)",
                    new { order.UserId, order.Freight, order.TotalAmount },
                    transaction);

                foreach (var item in order.Items)
                {
                    item.OrderId = orderId;
                    await connection.ExecuteAsync(@"
                            INSERT INTO OrderItem (OrderId, AnimalId, AnimalPrice)
                            VALUES (@OrderId, @AnimalId, @AnimalPrice)",
                        new { item.OrderId, item.AnimalId, item.AnimalPrice },
                        transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> IsAnimalAlreadyInOrder(int orderId, int animalId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var query = "SELECT COUNT(*) FROM OrderItem WHERE OrderId = @OrderId AND AnimalId = @AnimalId";
            var count = await connection.ExecuteScalarAsync<int>(query, new { OrderId = orderId, AnimalId = animalId });
            return count > 0;
        }
    }
}