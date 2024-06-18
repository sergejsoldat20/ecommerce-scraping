using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using scraper.Spiders.DTOs.InnSportDTO;

namespace backend.Spiders.DTOs
{
    public class InnSportProductWrapper
    {
        [JsonPropertyName("products")]
        public List<InnSportProduct> Products { get; set; } = new List<InnSportProduct>();
    }
}