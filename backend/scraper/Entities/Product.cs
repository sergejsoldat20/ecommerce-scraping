using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using scraper.Enums;

namespace scraper.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ProductUrl { get; set; } = string.Empty;
        public double Price { get; set; }
        public double PriceWithDiscount { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public Guid BrandId { get; set; }
        [JsonIgnore]
        public virtual Brand Brand { get; set; }
        public Gender Gender { get; set; }
        //public virtual Category Category { get; set; }
        //public Guid CategoryId { get; set; }
        [JsonIgnore]
        public List<ProductHistory> OldValuesList { get; set; } = new List<ProductHistory>();
    }
}