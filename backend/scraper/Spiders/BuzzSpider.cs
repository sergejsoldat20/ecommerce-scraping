using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HtmlAgilityPack;
using scraper.Data;
using scraper.Entities;
using scraper.Entities.DTO;
using scraper.Enums;
using scraper.Services;

namespace scraper.Spiders
{
    public class BuzzSpider : IBaseSpider
    {
        private readonly IBaseScraperService _baseScraperService;

        public BuzzSpider(IBaseScraperService baseScraperService)
        {
            _baseScraperService = baseScraperService;
        }

        public Task ScrapeBrandsFromShop()
        {
            return Task.CompletedTask;
        }
        
        public async Task ScrapeProductsFromShop()
        {
            await ScrapeFromBuzz();
            await ScrapeFromSportReality();
            await ScrapeFromSportVision();
        }

        public string FindImageFromTag(HtmlNode node, string baseUrl)
        {
            if (node != null)
            {
                HtmlNodeCollection imgNodes = node.SelectNodes(".//img");

                if (imgNodes != null)
                {
                    baseUrl += imgNodes.First().GetAttributeValue("src", "");
                }
            }
            return baseUrl;
        }

        public double FindPriceFromTag(HtmlNode node, bool hasDiscount)
        {
            double resultPrice = 0;
            if (node != null)
            {

                HtmlNode priceNode;
                // add to className 'current-price price-with-discount' for discount 			
                if (hasDiscount)
                {
                    priceNode = node.SelectSingleNode(".//div[@class='current-price price-with-discount']");
                }
                else
                {
                    priceNode = node.SelectSingleNode(".//div[@class='current-price ']");
                }

                if (priceNode != null)
                {
                    string price = priceNode
                        .InnerText
                        .Trim()
                        .Split(" ")[0]
                        .Replace(",", ".");
                    resultPrice = double.Parse(price, CultureInfo.InvariantCulture);
                }
            }
            return resultPrice;
        }

        public string FindTitleFromTag(HtmlNode node)
        {
            if (node != null)
            {
                HtmlNodeCollection linksCollection = node.SelectNodes(".//a");
                if (linksCollection != null)
                {
                    string title = linksCollection.First().GetAttributeValue("title", "");
                    return title;
                }
            }
            return string.Empty;
        }

        public string FindProductUrlFromTag(HtmlNode node)
        {
            if (node != null)
            {
                HtmlNodeCollection linksCollection = node.SelectNodes(".//a");
                if (linksCollection != null)
                {
                    string productUrl = linksCollection.First().GetAttributeValue("href", "");
                    return productUrl;
                }
            }
            return string.Empty;
        }

        public double FindOldPriceWithDiscount(HtmlNode node)
        {
            double resultPrice = 0;
            if (node != null)
            {
                // add to className 'current-price price-with-discount' for discount 
                HtmlNode priceNode = node.SelectSingleNode(".//div[starts-with(@class, 'prev-price')]");

                if (priceNode != null)
                {
                    string price = priceNode.InnerText.Trim().Split(" ")[0].Replace(",", ".");
                    resultPrice = double.Parse(price, CultureInfo.InvariantCulture);
                }
            }
            return resultPrice;
        }

        public int FindPaginationNumber(HtmlDocument document)
        {
            if (document != null)
            {
                HtmlNode node = document.DocumentNode.SelectSingleNode(".//a[@rel='last']");
                return Int32.Parse(node.InnerText);
            }
            return 0;
        }

        public async Task ExtractDataFromHtmlNodes(HtmlDocument document,
            string shopName, string baseUrl, string photoBaseUrl)
        {

            // get all brands 
            var brands = _baseScraperService.GetBrands();

            // var resultList = new List<Product>();
            var productElements = document.DocumentNode.SelectNodes("//div[@class='" + Consts.BuzzProductsDiv + "']");

            if (productElements != null)
            {
                // Console.WriteLine(productElements.Count);
                foreach (var div in productElements)
                {
                    double price = FindPriceFromTag(div, false);
                    double priceWithDiscount = FindPriceFromTag(div, true);
                    var gender = baseUrl.Contains("za-zene") ? Gender.FEMALE : Gender.MALE;

                    if (price == 0)
                    {
                        price = FindOldPriceWithDiscount(div);
                    }

                    Console.WriteLine(price);

                    // Construct product to save
                    var product = new ProductRequest
                    {
                        // Id = Guid.NewGuid(),
                        Name = FindTitleFromTag(div),
                        PhotoUrl = FindImageFromTag(div, photoBaseUrl),
                        Price = price,
                        PriceWithDiscount = priceWithDiscount,
                        ProductUrl = FindProductUrlFromTag(div),
                        ShopName = shopName,
                        Gender = gender
                    };

                    // update brand
                    var brand = _baseScraperService.FindBrandInShoeName(product.Name, brands);
                    product.BrandId = brand.Id;

                    // Save product to database
                    await _baseScraperService.SaveProductToDatabaseV2(product);
                    // Save product to elastic search
                    // await _baseScraperService.IndexNewProductAsync(product);

                }
            }
        }

        // Method that will collect everything from buzz
        public async Task ScrapeFromBuzz()
        {
            foreach (var baseUrl in Consts.BuzzURLList)
            {
                var web = new HtmlWeb();
                var document = web.Load(baseUrl);
                int paginationNumber = FindPaginationNumber(document);


                for (int i = 0; i < paginationNumber; i++)
                {
                    string baseUrlWithPage = baseUrl + $"page-{i}";
                    document = web.Load(baseUrlWithPage);

                    // Extract data from pages with pagination
                    // Add buzz for shop
                    await ExtractDataFromHtmlNodes(document,
                        "Buzz", baseUrl, Consts.BuzzUrl);
                }
            }

        }

        public async Task ScrapeFromSportVision()
        {
            foreach (var baseUrl in Consts.SportVisionURLList)
            {
                var web = new HtmlWeb();
                var document = web.Load(baseUrl);
                int paginationNumber = FindPaginationNumber(document);

                for (int i = 0; i < paginationNumber; i++)
                {
                    string baseUrlWithPage = baseUrl + $"page-{i}";
                    document = web.Load(baseUrlWithPage);

                    // Extract data from pages with pagination
                    // Add sport vision for shop
                    await ExtractDataFromHtmlNodes(document,
                         "Sport Vision", baseUrl, Consts.SportVisionUrl);
                }
            }
        }

        public async Task ScrapeFromSportReality()
        {
            foreach (var baseUrl in Consts.SportRealityURLList)
            {
                var web = new HtmlWeb();
                var document = web.Load(baseUrl);
                int paginationNumber = FindPaginationNumber(document);

                for (int i = 0; i < paginationNumber; i++)
                {
                    string baseUrlWithPage = baseUrl + $"page-{i}";
                    document = web.Load(baseUrlWithPage);

                    // Extract data from pages with pagination
                    // Add sport reality for shop
                    await ExtractDataFromHtmlNodes(document,
                        "Sport Reality", baseUrl, Consts.SportRealityUrl);

                }
            }
        }

    }
}