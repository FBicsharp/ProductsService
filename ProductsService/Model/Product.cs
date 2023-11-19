using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsService.Model
{
    public class Product
    {

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; } = Guid.NewGuid();
        [Key,Column(Order = 1),Required]
        public Guid company { get; set; } = Guid.Empty;
        [Key, Column(Order = 2), Required, MaxLength(35)]
        public string part_number { get; set; } = string.Empty;
        [Required, MaxLength(500)]
        public string description { get; set; } = string.Empty;
        [Required, MaxLength(10)]
        public string unit_of_measure { get; set; } = "Number";
        [Required]
        public decimal price { get; set; } = 0;
        [Required, MaxLength(10)]
        public string currency { get; set; } = string.Empty;
        [Required]
        public DateTime creation_date { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime last_update { get; set; } = DateTime.UtcNow;
        [MaxLength(63)]
        public string imageId { get; set; }
        public ProductState state { get; set; }


    }
    public enum ProductState
    {
        Active = 0,
        Inactive = 1

    }

}