using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.Configuration;

using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using scraper.Extensions;
using scraper.Services;


var builder = Host.CreateDefaultBuilder()
	.ConfigureAppConfiguration(
		(hosting, config) =>
		{
			//config.AddJsonFile("appsettings.Development.json", true);
			config.AddJsonFile("appsettings.json", true);
		}
	)
	.UseServiceProviderFactory(new AutofacServiceProviderFactory())
	.ConfigureServices(
		(context, services) =>
		{
			services.RegisterServices(context.Configuration);
		}
	);
using var host = builder.Build();

await host.RunAsync();