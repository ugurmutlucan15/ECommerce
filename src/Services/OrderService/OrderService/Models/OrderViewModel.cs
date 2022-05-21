namespace OrderService.Models
{
    public class OrderViewModel
    {
        public int UserId { get; set; }

        public string ProductId { get; set; }

        public decimal Price { get; set; }
    }
}