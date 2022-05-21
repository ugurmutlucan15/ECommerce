namespace CartService.Models
{
    public class CartViewModel
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public decimal ProductPrice { get; set; }
    }
}