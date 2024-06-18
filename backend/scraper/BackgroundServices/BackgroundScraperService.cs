using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using scraper.Factory;
using scraper.Spiders;

namespace scraper.BackgroundServices
{
    public class BackgroundScraperService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISpiderFactory _spiderFactory;
        private bool _isFirstRun = true;

        public BackgroundScraperService(IServiceProvider service,
            ISpiderFactory spiderFactory)
        {
            _serviceProvider = service;
            _spiderFactory = spiderFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_isFirstRun)
                {
                    await DoScheduledScraping();
                    _isFirstRun = false;
                    await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
                }

                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
                
            }
        }

        private async Task DoScheduledScraping()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var spiders = _spiderFactory.GetSpidersByConfig();

                if (spiders != null)
                {

                    foreach (var spider in spiders)
                    {
                        await spider.ScrapeBrandsFromShop();
                        await spider.ScrapeProductsFromShop();
                    }
                }
            }
        }
    }
}