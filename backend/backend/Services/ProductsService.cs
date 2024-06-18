using AutoMapper;
using backend.Data;
using backend.Entities;
using backend.Entities.DTO;
using backend.Enums;
using backend.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nest;


namespace backend.Services
{
	public interface IProductsService
	{
		List<Product> GetAllProductsFromDb();
		List<Product> FilterProductByPrice(double priceFrom, double priceTo);

		PageableList<ProductResponse> FilterProducts(FilterDTO filter, int pageSize, int pageNumber);
		List<string> GetAllShops();

		List<Brand> GetAllBrands();

		Task<Product> GetProductById(Guid id);

		Task<Brand> GetBrandById(Guid id);
		Task<List<ElsProductResponse>> GetRecommendedProducts(string productName, Guid productId);

	}

	public class ProductsService : IProductsService
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly IElasticClient _elasticClient;

		public ProductsService(ApplicationDbContext context, IMapper mapper,
			IElasticClient elasticClient)
		{
			_mapper = mapper;
			_context = context;
			_elasticClient = elasticClient;
		}

		public List<Product> FilterProductByPrice(double priceFrom, double priceTo)
		{

			throw new NotImplementedException();
		}

		public PageableList<ProductResponse> FilterProducts(FilterDTO filter, int pageSize, int pageNumber)
		{

			filter = CheckFilter(filter);
			if (filter != null)
			{
				var query = _context.Products.AsQueryable();

				if (!string.IsNullOrEmpty(filter.shopName))
				{
					query = query.Where(p => p.ShopName.Equals(filter.shopName));
				}

				query = query.Where(p => p.Price >= filter.priceFrom && p.Price <= filter.priceTo);

				if (!string.IsNullOrEmpty(filter.gender))
				{
					if (Enum.TryParse(typeof(Gender), filter.gender, out var genderValue))
					{
						query = query.Where(p => p.Gender == (Gender)genderValue);
					}
				}

				if (filter.brandId != Guid.Empty)
				{
					query = query.Where(p => p.BrandId.Equals(filter.brandId));
				}

				if(!string.IsNullOrEmpty(filter.searchString)) 
				{
					query = query.AsEnumerable()
					.Where(p => p.Name.Contains(filter.searchString, StringComparison.InvariantCultureIgnoreCase))
					.AsQueryable();
				}


				if (filter.sortType.Equals(SortType.BY_NAME))
				{
					query = query.OrderBy(p => p.Name);
				}
				else if (filter.sortType.Equals(SortType.BY_PRICE_DESC))
				{
					query = query.OrderByDescending(p => p.Price);
				}
				else if (filter.sortType.Equals(SortType.BY_PRICE_ASC))
				{
					query = query.OrderBy(x => x.Price);
				}



				// Create pages for infinite scroll after filters
				List<Product> listProducts = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
				List<ProductResponse> response = _mapper.Map<List<ProductResponse>>(listProducts);
				var result = new PageableList<ProductResponse>(response, query.ToList().Count, pageNumber, pageSize);


				return result;
			}
			else
			{
				throw new ApplicationException("Filter is null!");
			}
		}

		public async Task<List<ElsProductResponse>> GetRecommendedProducts(string productName, Guid productId)
		{
			var response = await _elasticClient.SearchAsync<ElsProductResponse>(s => s
				.Query(q => q
					.Bool(b => b
						.Must(mu => mu
							.MoreLikeThis(m => m
								.Fields(f => f.Field(ff => ff.Name))
								.Like(l => l.Text(productName))
								.MinTermFrequency(1)
								.MaxQueryTerms(12)))
					)
				)
				.From(0)
				.Size(10) // Retrieve 10 products
			);


			if (!response.IsValid)
			{
				throw new AppException("Error executing MLT in elasticsearch for recommended products");
			}

			return response.Documents.Where(x => x.Id != productId).ToList();
		}

		public List<Brand> GetAllBrands()
		{
			var result = _context.Brands.ToList();
			return result;
		}

		public List<Product> GetAllProductsFromDb()
		{
			return _context
				.Products
				.Take(20)
				.ToList();
		}

		public List<string> GetAllShops()
		{
			var result = _context
				.Products
				.Select(x => x.ShopName)
				.Distinct()
				.ToList();

			return result;
		}

		public async Task<Brand> GetBrandById(Guid id)
		{
			var result = await _context
				.Brands.FirstAsync(x => x.Id == id);
			return result;
		}

		public async Task<Product> GetProductById(Guid id)
		{
			var result = await _context
				.Products.FirstAsync(x => x.Id == id);
			return result;

		}

		public List<Product> ScrapeFromAll()
		{
			throw new NotImplementedException();
		}

		public List<Product> SearchByName(string name)
		{
			throw new NotImplementedException();
		}

		private FilterDTO CheckFilter(FilterDTO filter)
		{
			var changedFilter = new FilterDTO
			{
				priceFrom = filter.priceFrom == 0 ? 0 : filter.priceFrom,
				priceTo = filter.priceTo == 0 ? Int32.MaxValue : filter.priceTo,
				gender = filter.gender,
				shopName = filter.shopName,
				brandId = filter.brandId,
				sortType = filter.sortType,
				searchString = filter.searchString

			};
			return changedFilter;

		}

	}
}
