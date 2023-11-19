using ProductsService.Model;

namespace ProductsService.Dtos
{
    //notify object for external services
    public class ProductNotifiyDto
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
        public ProductEventType EventType { get; set; }

    }
    public enum ProductEventType
    {
        Created_Event = 0,
        Updated_Event = 1,
        Deleted_Event = 2



    }
}
