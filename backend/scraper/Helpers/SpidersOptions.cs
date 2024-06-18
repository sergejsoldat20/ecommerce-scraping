using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scraper.Helpers
{
    public class SpidersOptions
    {
        public const string SectionName = "SpidersOptions";
        public required List<string> ScrapingShops { get; set; }
    }
}