using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsService.Model
{
    public class Image
    {
                
        public Guid id { get; set; } = Guid.NewGuid();
        [Key,Column(Order = 1),Required]
        public Guid company { get; set; } = Guid.Empty;
        [Key, Column(Order = 2), Required, MaxLength(63)]
        public string name { get; set; } = string.Empty;
        [Required, MaxLength(500)]
        public string description { get; set; } = string.Empty;        
        [Required]
        public DateTime creation_date { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime last_update { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// BlobStorege Id
        /// </summary>
        [MaxLength(63)]
        public string external_storege_id { get; set; }
        public string image_extension { get; set; } //"jpeg"
        public byte[] image_content { get; set; }


    }
    

}