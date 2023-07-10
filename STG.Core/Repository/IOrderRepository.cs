using STG.Core.Entities;

namespace STG.Core.Repository
{
    public interface IOrderRepository
    {
        Task<Order> GetById(int orderId);

        Task Create(Order order);

        Task<bool> IsAnimalAlreadyInOrder(int orderId, int animalId);
    }
}