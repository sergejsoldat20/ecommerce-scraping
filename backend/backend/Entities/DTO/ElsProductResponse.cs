using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Enums;

namespace backend.Entities.DTO
{
    public class ElsProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double PriceWithDiscount { get; set; }
        public string PhotoUrl { get; set; }
        public string ShopName { get; set; }
        public Gender Gender { get; set; }
    }
}