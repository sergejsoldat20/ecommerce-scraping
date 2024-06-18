using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using scraper.Helpers;
using scraper.Spiders;

namespace scraper.Factory
{
    public interface ISpiderFactory
    {
        IBaseSpider GetSpiderByName(string name);
        List<IBaseSpider> GetSpidersByConfig();
    }
    public class SpiderFactory : ISpiderFactory
    {
        private readonly IEnumerable<IBaseSpider> _spiders;
        private readonly SpidersOptions _options;

        public SpiderFactory(IEnumerable<IBaseSpider> spiders,
            IOptions<SpidersOptions> options)
        {
            _spiders = spiders;
            _options = options.Value;
        }

        public IBaseSpider GetSpiderByName(string name)
        {
            return _spiders.FirstOrDefault(s => s.GetType().Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?? throw new KeyNotFoundException($"Spider with name {name} not found!");
        }

        public List<IBaseSpider> GetSpidersByConfig()
        {
            var selectedSpiders = new List<IBaseSpider>();
            if (_options.ScrapingShops.Count > 0)
            {
                foreach (var name in _options.ScrapingShops)
                {
                    var spider = _spiders
                        .FirstOrDefault(x => x.GetType().Name
                        .Equals(name, StringComparison.OrdinalIgnoreCase));

                    if (spider != null)
                    {
                        selectedSpiders.Add(spider);
                    }
                    else
                    {
                        throw new KeyNotFoundException($"Spider with name {name} not found in get all spiders!");
                    }
                }
            }


            return selectedSpiders;
        }
    }
}