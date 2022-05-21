namespace CartService.Entities
{
    public class Cart
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public decimal ProductPrice { get; set; }
    }
}