using System.Text.Json.Serialization;

namespace scraper.Spiders.DTOs.JuventaDTO
{
	public class BrandDTO
	{
		[JsonPropertyName("id")]
		public string Id { get; set; } = string.Empty;
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
	}
}
