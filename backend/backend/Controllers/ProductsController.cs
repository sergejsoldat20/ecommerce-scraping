using backend.Entities;
using backend.Entities.DTO;
using backend.Enums;
using backend.Helpers;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
	private readonly IProductsService _productsService;

	public ProductsController(IProductsService productsService)
	{
		_productsService = productsService;
	}

	[HttpGet("get-by-id/{id}")]
	public async Task<ActionResult<Product>> GetProductById(Guid id)
	{ 
		var result = await _productsService.GetProductById(id);

		if (result == null) 
		{
			return NotFound();
		}
		return result;
	}

	[HttpPost("filter")]
	public ActionResult<PageableList<ProductResponse>> FilterProducts(FilterDTO payload,
		[FromQuery] int pageSize = 20, 
		[FromQuery] int pageNumber = 1)
	{
		var response = _productsService.FilterProducts(payload, pageSize, pageNumber);
		return Ok(response);
	}

	[HttpGet("shop-names")]
	public ActionResult<List<string>> GetAllShopNames()
	{
		var response = _productsService.GetAllShops();

		if (response == null)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("brands")]
	public ActionResult<List<string>> GetAllBrands()
	{
		var response = _productsService.GetAllBrands();

		if (response == null)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("brand-by-id/{id}")]
	public async Task<ActionResult<Brand>> GetBrandById(Guid id) 
	{
		var response = await _productsService.GetBrandById(id);

		if (response == null)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpPost("product-recommendation")]
	public async Task<ActionResult<List<ElsProductResponse>>> GetRecommendations(RecomendationsRequest request) 
	{
		
		var response = await _productsService.GetRecommendedProducts(request.Name, request.Id);

		if (response == null) 
		{
			return NotFound();
		}
		return Ok(response);
	}
}
