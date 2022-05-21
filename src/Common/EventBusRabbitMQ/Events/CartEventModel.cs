namespace EventBusRabbitMQ.Events
{
    public class CartEventModel
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public decimal ProductPrice { get; set; }
    }
}