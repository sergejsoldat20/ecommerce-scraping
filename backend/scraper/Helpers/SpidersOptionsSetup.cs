using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace scraper.Helpers
{
    internal sealed class SpidersOptionsSetup : IConfigureOptions<SpidersOptions>
    {
        private readonly IConfiguration _configuration;

        public SpidersOptionsSetup(IConfiguration configuration) => _configuration = configuration;

        // configure appsettings
        public void Configure(SpidersOptions options) => _configuration
            .GetSection(SpidersOptions.SectionName).Bind(options);
    }
}