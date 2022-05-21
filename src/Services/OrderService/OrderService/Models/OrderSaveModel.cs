namespace OrderService.Models
{
    public class OrderSaveModel
    {
        public int UserId { get; set; }

        public string ProductId { get; set; }

        public decimal Price { get; set; }
    }
}