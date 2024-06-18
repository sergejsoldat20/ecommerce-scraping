using Newtonsoft.Json;

namespace scraper.Scrapers.DTOs.JuventaDTO
{
	public class ProductWrapper
	{
		[JsonProperty("products")]
		public ProductsDTO Products { get; set; }
	}
}
