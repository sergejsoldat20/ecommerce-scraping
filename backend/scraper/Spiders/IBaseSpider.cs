using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using scraper.Entities;

namespace scraper.Spiders
{
    public interface IBaseSpider
    {
        Task ScrapeProductsFromShop();
        Task ScrapeBrandsFromShop();
    }
}