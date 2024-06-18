using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Nest;
using Newtonsoft.Json.Bson;
using scraper.Data;
using scraper.DTOs;
using scraper.Entities;
using scraper.Entities.DTO;
using scraper.Helpers;

namespace scraper.Services
{
    public interface IBaseScraperService
    {
        List<Product> GetAllProductsFromShop();
        // Task SaveProductToDatabase(Product product);
        bool CheckIfDatabaseHasProducts();
        Task SaveBrandToDatabase(Brand brand);
        bool CheckIfBrandsExist();
        List<Brand> GetBrands();
        Brand FindBrandInShoeName(string shoeName, List<Brand> brands);
        Guid GetBrandId(string name, List<Brand> brands);
        // Task SaveProductToElasticSearch(Product product);
        // Task<ISearchResponse<Product>> GetFromELS();
        Task IndexNewProductAsync(Product product);
        Task SaveProductToDatabaseV2(ProductRequest productRequest);
    }

    public class BaseScraperService : IBaseScraperService
    {

        private readonly ApplicationDbContext _context;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public BaseScraperService(ApplicationDbContext context,
            IElasticClient elasticClient,
            IMapper mapper)
        {
            _elasticClient = elasticClient;
            _context = context;
            _mapper = mapper;
        }

        public bool CheckIfBrandsExist()
        {
            return _context.Brands.Any();
        }

        public bool CheckIfDatabaseHasProducts()
        {
            return _context.Products.Any();
        }

        public Brand FindBrandInShoeName(string shoeName, List<Brand> brands)
        {
            // we send brands and name of product, if there is brand name in product return brand
            foreach (var brand in brands)
            {
                if (shoeName.Contains(brand.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return brand;
                }
            }

            // there is no name brand if method can recognize
            return brands.First(x => x.Name == "NO NAME");
        }

        public List<Product> GetAllProductsFromShop()
        {
            throw new NotImplementedException();
        }

        public Guid GetBrandId(string name, List<Brand> brands)
        {
            return brands.First(x => x.Name == name).Id;
        }

        public List<Brand> GetBrands()
        {
            var result = _context.Brands.ToList();
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new AppException("Can't find any brands");
            }
        }

        public async Task SaveBrandToDatabase(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task SaveProductToDatabaseV2(ProductRequest productRequest)
        {
            if (productRequest == null)
            {
                throw new AppException("ProductRequest is null");
            }

            var existingProduct = _context.Products
                .FirstOrDefault(x => x.Name == productRequest.Name
                                && x.ShopName == productRequest.ShopName
                                && x.Gender == productRequest.Gender);

            if (existingProduct == null)
            {
                Product product;

                try
                {
                    // do mapping of product request to product
                    product = _mapper.Map<Product>(productRequest);
                }
                catch
                {
                    throw new AppException("Product request not mapped in Product");
                }

                product.Id = Guid.NewGuid();
                _context.Products.Add(product);
                _context.SaveChanges();
                // when added to DB add it in elastic search
                await IndexNewProductAsync(product);
            }

            if (existingProduct != null)
            {
                if (existingProduct.Price != productRequest.Price
                    || existingProduct.PriceWithDiscount != productRequest.PriceWithDiscount)
                {
                    existingProduct.Price = productRequest.Price;
                    existingProduct.PriceWithDiscount = productRequest.PriceWithDiscount;
                    await _context.SaveChangesAsync();
                }
            }
        }


        // public async Task SaveProductToDatabase(Product product)
        // {

        //     if (product != null)
        //     {
        //         var existingProduct = _context.Products
        //             .FirstOrDefault(p => p.Name == product.Name
        //             && p.ShopName == product.ShopName
        //             && p.Gender == product.Gender);



        //         if (existingProduct == null)
        //         {
        //             _context.Products.Add(product);
        //         }
        //         else
        //         {
        //             if (!ProductsAreEqual(existingProduct, product))
        //             {
        //                 // TODO: Add price with disconunt in history
        //                 var history = new ProductHistory
        //                 {
        //                     ProductId = existingProduct.Id,
        //                     OldValue = existingProduct.Price,
        //                     NewValue = product.Price,
        //                     Id = Guid.NewGuid(),
        //                     // ChangeGroup = groupGuid,
        //                     Timestamp = DateTime.UtcNow
        //                 };

        //                 // Update product history
        //                 await _context.AddAsync(history);

        //                 // Update product current price
        //                 existingProduct.Price = product.Price;
        //                 existingProduct.PriceWithDiscount = product.PriceWithDiscount;
        //                 _context.Update(existingProduct);

        //             }
        //         }
        //         await _context.SaveChangesAsync();
        //     }
        // }

        private bool ProductsAreEqual(Product a, Product b)
        {
            return a.Name.Equals(b.Name)
                && a.ProductUrl.Equals(b.ProductUrl)
                && a.Gender == b.Gender
                //&& a.PhotoUrl == b.PhotoUrl
                && a.ShopName.Equals(b.ShopName)
                && a.PriceWithDiscount == b.PriceWithDiscount
                && a.Price == b.Price;
        }

        public async Task IndexNewProductAsync(Product product)
        {
            var elsProduct = new ElsProduct
            {
                Name = product.Name,
                Id = product.Id,
                Price = product.Price,
                PriceWithDiscount = product.PriceWithDiscount,
                ShopName = product.ShopName,
                PhotoUrl = product.PhotoUrl,
                Gender = product.Gender
            };
            var indexResponse = await _elasticClient.IndexDocumentAsync(elsProduct);
            if (!indexResponse.IsValid)
            {
                throw new InvalidOperationException("Failed to index the new product in Elasticsearch.");
            }
        }
    }
}