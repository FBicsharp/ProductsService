﻿namespace ProductsService.Dtos
{
    public class ProductCreateDto
    {
        public Guid company { get; set; }
        public string part_number { get; set; }
        public string description { get; set; }
        public string unit_of_measure { get; set; }
        public decimal price { get; set; }
        public string currency { get; set; }
        public string imageId { get; set; }

    }
}
