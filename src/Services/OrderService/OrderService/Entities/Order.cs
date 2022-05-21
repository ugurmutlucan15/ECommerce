namespace OrderService.Entities
{
    public class Order
    {
        public int UserId { get; set; }

        public string ProductId { get; set; }

        public decimal Price { get; set; }
    }
}