using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models
{
    public class ProductSaveModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}