using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using scraper.Entities;
using scraper.Entities.DTO;
using scraper.Enums;
using scraper.Helpers;
using scraper.Scrapers.DTOs.JuventaDTO;
using scraper.Services;
using scraper.Spiders.DTOs.JuventaDTO;

namespace scraper.Spiders
{
    public class JuventaSpider : IBaseSpider
    {
        private readonly IBaseScraperService _baseScraperService;

        public JuventaSpider(IBaseScraperService baseScraperService)
        {
            _baseScraperService = baseScraperService;
        }

        public async Task ScrapeBrandsFromShop()
        {
            await ScrapeBrands();
        }
        public async Task ScrapeProductsFromShop()
        {
            await ScrapeFromJuventa(Consts.JuventaMaleProductsUrl);
            await ScrapeFromJuventa(Consts.JuventaFemaleProductsUrl);
        }

        public async Task ScrapeFromJuventa(string url)
        {

            // we use first page to get number of pages
            string urlOfFirstPage = url + "1";
            ProductWrapper wrapper = await DeserializeSinglePage(urlOfFirstPage);

            // count how much pages we have
            int numberOfPages = (wrapper.Products.Total / wrapper.Products.PerPage) % wrapper.Products.PerPage == 0 ?
                (wrapper.Products.Total / wrapper.Products.PerPage) : (wrapper.Products.Total / wrapper.Products.PerPage) + 1;

            for (int i = 0; i < numberOfPages; i++)
            {
                string pageUrl = url + $"{i}";
                ProductWrapper productWrapper = await DeserializeSinglePage(pageUrl);
                Gender gender = url.Contains(Consts.JuventaFemaleId) ? Gender.FEMALE : Gender.MALE;
                Console.WriteLine(gender.ToString());
                await ConvertToProducts(productWrapper, gender);
            }
        }

        public async Task ConvertToProducts(ProductWrapper wrapper, Gender gender)
        {
            var brands = _baseScraperService.GetBrands();
            foreach (var product in wrapper.Products.Data)
            {

                var newProduct = new ProductRequest
                {
                    Name = product.Name,
                    Price = product.FirstPrice,
                    PriceWithDiscount = product.Price == product.FirstPrice ? 0 : product.Price,
                    ProductUrl = Consts.JuventaSingleProductUrl + product.SearchableId,
                    PhotoUrl = Consts.JuventaBaseUrl + product.HeroThumb,
                    ShopName = Consts.JuventaName,
                    BrandId = _baseScraperService.GetBrandId(product.Brand.Name, brands),
                    Gender = gender
                };
                Console.WriteLine(product.Name);
                Console.WriteLine(newProduct.BrandId);

                await _baseScraperService.SaveProductToDatabaseV2(newProduct);
                
            }
        }

        // this function will scrape single page from Juventa
        public async Task<ProductWrapper> DeserializeSinglePage(string pageUrl)
        {
            HttpClient client = new HttpClient();
            // client.BaseAddress = new Uri(pageUrl);

            var response = await client.GetAsync(pageUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                ProductWrapper productWrapper;

                if (jsonResponse != null)
                {
                    productWrapper = JsonConvert.DeserializeObject<ProductWrapper>(jsonResponse);
                }
                else
                {
                    throw new AppException("Json response is null!");
                }

                if (productWrapper != null)
                {
                    return productWrapper;
                }
                else
                {
                    throw new AppException("Parsed product is null!");
                }
            }
            else
            {
                throw new AppException("Response status for Juventa Scraper is not 200!");
            }
        }

        public async Task ScrapeBrands()
        {
            if (!_baseScraperService.CheckIfBrandsExist())
            {
                var result = new List<BrandDTO>();

                HttpClient httpClient = new HttpClient();

                // form base api url we can get all brands 
                var response = await httpClient.GetAsync(Consts.JuventaAPIBaseURL);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // parse to JObject 
                    JObject data = JObject.Parse(responseBody);

                    JToken filters = data["filters"]["brands"]["data"];

                    result = filters.ToObject<List<BrandDTO>>();

                    if (result != null)
                    {
                        foreach (var brand in result)
                        {
                            var newBrand = new Brand
                            {
                                Id = Guid.NewGuid(),
                                Name = brand.Name
                            };
                            await _baseScraperService.SaveBrandToDatabase(newBrand);

                        }
                    }

                }
            }

        }
    }
}