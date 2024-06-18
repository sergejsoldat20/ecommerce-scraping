using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using scraper.BackgroundServices;
using scraper.Data;
using scraper.Factory;
using scraper.Helpers;
using scraper.Services;
using scraper.Spiders;

namespace scraper.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {

            // register options 
            services.ConfigureOptions<SpidersOptionsSetup>();
            // register db
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("Db"))
            );

            var settings = new ConnectionSettings(new Uri("http://192.168.100.15:9200"))
                .DefaultIndex("products");

            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);

            // configure automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // register services
            services.AddScoped<IBaseScraperService, BaseScraperService>();
            services.AddTransient<IBaseSpider, BuzzSpider>();
            services.AddTransient<IBaseSpider, JuventaSpider>();
            services.AddTransient<IBaseSpider, InnSportSpider>();


            services.AddSingleton<ISpiderFactory, SpiderFactory>();
            services.AddHostedService<BackgroundScraperService>();


            return services;
        }
    }
}