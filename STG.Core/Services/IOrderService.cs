using STG.Core.Entities;

namespace STG.Core.Services
{
    public interface IOrderService
    {
        Task<Order> PlaceOrder(Order order);
        Task<bool> IsAnimalAlreadyInOrder(int orderId, int animalId);
    }
}
