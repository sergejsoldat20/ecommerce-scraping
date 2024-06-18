using Newtonsoft.Json;

namespace scraper.Scrapers.DTOs.JuventaDTO
{
	public class ProductsDTO
	{
		[JsonProperty("page")]
		public int Page { get; set; }

		[JsonProperty("per_page")]
		public int PerPage { get; set; }

		[JsonProperty("data")]
		public List<Root> Data { get; set; }

		[JsonProperty("total")]
		public int Total { get; set; }

	}
}
