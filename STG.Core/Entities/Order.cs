namespace STG.Core.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public List<Animal> Animals { get; set; }
        public decimal TotalAmount { get; set; }
        public int Freight { get; set; }
    }
}
