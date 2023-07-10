using STG.Core.Entities;
using STG.Core.Repository;
using STG.Core.Services;

namespace STG.Application
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAnimalRepository _animalRepository;

        public OrderService(IOrderRepository orderRepository, IAnimalService animalService, IAnimalRepository animalRepository)
        {
            _orderRepository = orderRepository;
            _animalRepository = animalRepository;
        }

        public async Task<Order> PlaceOrder(Order order)
        {
            CalculateOrderTotalAmount(order);
            ApplyDiscountsAndFreight(order);
            await _orderRepository.Create(order);
            return order;
        }

        public async Task<bool> IsAnimalAlreadyInOrder(int orderId, int animalId)
        {
            return await _orderRepository.IsAnimalAlreadyInOrder(orderId, animalId);
        }

        private async void CalculateOrderTotalAmount(Order order)
        {
            decimal totalAmount = 0;
            foreach (var item in order.Items)
            {
                var animal = (await _animalRepository.FilterAnimals(item.AnimalId).ConfigureAwait(false)).First();
                item.AnimalPrice = animal.Price;
                totalAmount += animal.Price;
            }
            order.TotalAmount = totalAmount;
        }

        private static void ApplyDiscountsAndFreight(Order order)
        {
            int totalQuantity = order.Items.Count;
            decimal totalAmount = order.TotalAmount;

            if (totalQuantity > 50)
            {
                decimal discountPercentage = 0.05m;
                decimal discountAmount = totalAmount * discountPercentage;
                totalAmount -= discountAmount;
            }

            if (totalQuantity > 200)
            {
                decimal additionalDiscountPercentage = 0.03m;
                decimal additionalDiscountAmount = totalAmount * additionalDiscountPercentage;
                totalAmount -= additionalDiscountAmount;
            }

            if (totalQuantity > 300)
            {
                order.TotalAmount = totalAmount;
                order.Freight = 0;
            }
            else
            {
                order.TotalAmount = totalAmount + 1000;
                order.Freight = 1000;
            }
        }
    }
}