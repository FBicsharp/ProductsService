using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsService.Model
{
    public class Product
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key,Column(Order = 1),Required]
        public Guid Company { get; set; }
        [Key, Column(Order = 2), Required, MaxLength(35)]
        public string Article { get; set; }
        [Required, MaxLength(500)]
        public string Description { get; set; }
        [Required, MaxLength(3)]
        public string Uom { get; set; }
        [Required]
        public decimal Price { get; set; } = 0;
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        [MaxLength(63)]
        public string ImageId { get; set; }

    }
}