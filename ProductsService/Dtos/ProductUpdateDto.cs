using ProductsService.Model;

namespace ProductsService.Dtos
{
    //for future service
    public class ProductUpdateDto
    {
        public Guid company { get; set; }
        public Guid id { get; set; }
        public string part_number { get; set; }
        public string description { get; set; }
        public string unit_of_measure { get; set; }
        public decimal price { get; set; }
        public string currency { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime last_update { get; set; }
        public string imageId { get; set; }
        public ProductState state { get; set; }

    }
}
