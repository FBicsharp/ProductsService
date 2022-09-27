namespace ProductsService.Dtos
{
    public class ProductReadDto
    {
        public int Id { get; set; }

        public string Article { get; set; }
        public string Description { get; set; }
        public string Uom { get; set; }
        public decimal Price { get; set; }
        public string ImageId { get; set; }

    }
}
