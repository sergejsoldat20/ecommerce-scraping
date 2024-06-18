using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using scraper.Enums;

namespace scraper.Entities.DTO
{
    public class ProductRequest
    {
         public string Name { get; set; } = string.Empty;
        public string ProductUrl { get; set; } = string.Empty;
        public double Price { get; set; }
        public double PriceWithDiscount { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public Guid BrandId { get; set; }
        public Gender Gender { get; set; }
    }
}