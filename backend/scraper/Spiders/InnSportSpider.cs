using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using backend.Spiders.DTOs;
using Newtonsoft.Json;
using scraper.Entities;
using scraper.Entities.DTO;
using scraper.Enums;
using scraper.Helpers;
using scraper.Services;

namespace scraper.Spiders
{
    public class InnSportSpider : IBaseSpider
    {
        private readonly IBaseScraperService _baseScraperService;

        public InnSportSpider(IBaseScraperService baseScraperService)
        {
            _baseScraperService = baseScraperService;
        }

        public Task ScrapeBrandsFromShop()
        {
            return Task.CompletedTask;
        }

        public async Task ScrapeProductsFromShop()
        {
            await GetAllProductsFromShop();
        }

        public async Task GetAllProductsFromShop()
        {
            int counter = 1;
            // InnSportProductWrapper wrapper;
            while (true)
            {
                string pageUrl = Consts.InnSportProducts + $"{counter}";
                InnSportProductWrapper wrapper = await DeserializeSinglePage(pageUrl);
                Console.WriteLine(counter);
                if (wrapper.Products.Count == 0)
                {
                    break;
                }
                else
                {
                    counter++;
                    await ConvertToProducts(wrapper);
                }

            }
        }

        public async Task<InnSportProductWrapper> DeserializeSinglePage(string pageUrl)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");

            var response = await httpClient.GetAsync(pageUrl);


            if (!response.IsSuccessStatusCode)
            {
                throw new AppException("Get request is bad INN SPORT");
            }
            string jsonResponse = await response.Content.ReadAsStringAsync();
            InnSportProductWrapper productWrapper;

            if (jsonResponse == null)
            {
                throw new AppException("Json response is null INN SPORT");
            }

            productWrapper = JsonConvert.DeserializeObject<InnSportProductWrapper>(jsonResponse);

            if (productWrapper == null)
            {
                throw new AppException("Parsed product is null INN SPORT");
            }
            return productWrapper;
        }

        public async Task ConvertToProducts(InnSportProductWrapper productWrapper)
        {
            List<Brand> allBrands = _baseScraperService.GetBrands();

            if (productWrapper != null)
            {
                foreach (var product in productWrapper.Products)
                {
                    // if product is male product then url would be different than female
                    string productUrl = product.Tags.Contains(Consts.InnSportMaleTag) ?
                        Consts.InnSportMaleProductBaseURL + product.Handle :
                        Consts.InnSportFemaleProductBaseURL + product.Handle;


                    var newProduct = new ProductRequest
                    {
                        Name = product.Title,
                        // find brand name inside title
                        BrandId = _baseScraperService.FindBrandInShoeName(product.Title, allBrands).Id,
                        // price is string inside scraped dto so it has to be parsed 
                        Price = Double.Parse(product.Variants[0].Price, CultureInfo.InvariantCulture),
                        PriceWithDiscount = 0,
                        // more images are provided so we user first s
                        PhotoUrl = product.Images[0].Src,
                        ProductUrl = productUrl,
                        ShopName = Consts.InnSportShopName,
                        // tags contains info about gender
                        Gender = product.Tags.Contains(Consts.InnSportMaleTag) ? Gender.MALE : Gender.FEMALE
                    };

                    await _baseScraperService.SaveProductToDatabaseV2(newProduct);
                    // await _baseScraperService.IndexNewProductAsync(newProduct);

                }
            }
        }
    }
}