using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using scraper.Entities;
using scraper.Entities.DTO;

namespace scraper.Helpers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductRequest, Product>();
        }
    }
}