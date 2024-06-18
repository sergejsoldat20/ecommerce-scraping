using scraper.Spiders.DTOs.JuventaDTO;
using Newtonsoft.Json;

namespace scraper.Scrapers.DTOs.JuventaDTO;

public class Root
{
	[JsonProperty("searchable_id")]
	public string SearchableId { get; set; }

	//[JsonProperty("images")]
	//public List<string> Images { get; set; }

	//[JsonProperty("tabs")]
	//public List<Tab> Tabs { get; set; }

	[JsonProperty("active")]
	public int Active { get; set; }

	[JsonProperty("created_at")]
	public DateTime CreatedAt { get; set; }

	[JsonProperty("hero")]
	public string Hero { get; set; }

	[JsonProperty("price_updated_at")]
	public DateTime PriceUpdatedAt { get; set; }

	[JsonProperty("updated_at")]
	public DateTime UpdatedAt { get; set; }

	[JsonProperty("first_price")]
	public int FirstPrice { get; set; }

	[JsonProperty("price")]
	public double Price { get; set; }

	[JsonProperty("original_name")]
	public string OriginalName { get; set; }

	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("sku")]
	public string Sku { get; set; }

	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("hero_thumb")]
	public string HeroThumb { get; set; }

	//[JsonProperty("badges")]
	//public Badges Badges { get; set; }

	[JsonProperty("discount")]
	public int Discount { get; set; }

	[JsonProperty("original_price")]
	public int OriginalPrice { get; set; }

	//[JsonProperty("tags")]
	//public List<Tag> Tags { get; set; }

	[JsonProperty("brand")]
	public BrandDTO Brand { get; set; }
}

//public class Tab
//{
//	[JsonProperty("title")]
//	public string Title { get; set; }

//	[JsonProperty("content")]
//	public string Content { get; set; }
//}

//public class Badges
//{
//	public class Badge2
//	{
//		[JsonProperty("image")]
//		public string Image { get; set; }

//		[JsonProperty("position")]
//		public string Position { get; set; }
//	}

//	[JsonProperty("2")]
//	public Badge2 BadgeTwo { get; set; }
//}

//public class Brand
//{
//	[JsonProperty("name")]
//	public string Name { get; set; }	
//}

//public class Tag
//{
//	[JsonProperty("searchable_id")]
//	public string SearchableId { get; set; }

//	[JsonProperty("webshop_name")]
//	public string WebshopName { get; set; }

//	[JsonProperty("updated_at")]
//	public string UpdatedAt { get; set; }

//	[JsonProperty("name")]
//	public string Name { get; set; }

//	[JsonProperty("created_at")]
//	public string CreatedAt { get; set; }

//	[JsonProperty("webshop_filter")]
//	public int WebshopFilter { get; set; }

//	[JsonProperty("order")]
//	public int Order { get; set; }

//	[JsonProperty("id")]
//	public int Id { get; set; }
//}