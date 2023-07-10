namespace STG.Core.Entities
{
    public class OrderItem
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public int AnimalId { get; set; }
        public decimal AnimalPrice { get; set; }
    }
}