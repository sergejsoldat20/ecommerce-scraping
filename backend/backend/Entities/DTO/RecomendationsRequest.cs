using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Entities.DTO
{
    public class RecomendationsRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
       /// public string ShopName { get; set; }
    }
}