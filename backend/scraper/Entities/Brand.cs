using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scraper.Entities
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}