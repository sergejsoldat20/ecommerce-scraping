using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace scraper.Spiders.DTOs.InnSportDTO
{
    public class InnSportProduct
    {
        // public Guid Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;
        [JsonProperty("handle")]
        public string Handle { get; set; } = string.Empty;
        [JsonProperty("tags")]
        public List<string> Tags { get; set; } = new List<string>();
        [JsonProperty("images")]
        public List<InnSportImage> Images { get; set; } = new List<InnSportImage>();
        [JsonProperty("variants")]
        public List<InnSportVariants> Variants { get; set; } = new List<InnSportVariants>();

    }

    public class InnSportImage
    {
        [JsonProperty("src")]
        public string Src { get; set; } = string.Empty; 
    }

    public class InnSportVariants 
    {
        [JsonProperty("price")]
        public string Price { get; set; } = string.Empty;
        [JsonProperty("compare_at_price")] 
        public string CompareAtPrice { get; set; } = string.Empty;
    }
}