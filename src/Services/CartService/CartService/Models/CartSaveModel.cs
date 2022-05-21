using System.ComponentModel.DataAnnotations;

namespace CartService.Models
{
    public class CartSaveModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }
    }
}