using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using scraper.Enums;

namespace scraper.DTOs
{
    public class ElsProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public double Price { get; set; }
        public double PriceWithDiscount { get; set; }
        public string ShopName { get; set; }
        public Gender Gender {get; set; }
    }
}