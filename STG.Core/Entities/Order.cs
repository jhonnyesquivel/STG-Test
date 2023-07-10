namespace STG.Core.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public int Freight { get; set; }
    }
}