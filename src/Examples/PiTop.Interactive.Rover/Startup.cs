using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pocket;

using static Pocket.Logger;
namespace PiTop.Interactive.Rover
{
    public class Startup
    {
        public Startup(
            IHostEnvironment env,
            HttpOptions httpOptions)
        {
            Environment = env;
            HttpOptions = httpOptions;

            var configurationBuilder = new ConfigurationBuilder();

            Configuration = configurationBuilder.Build();
        }

        protected IConfigurationRoot Configuration { get; }

        protected IHostEnvironment Environment { get; }

        public HttpOptions HttpOptions { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            using var _ = Log.OnEnterAndExit();
            services.AddDotnetInteractiveHttpApi();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostApplicationLifetime lifetime,
            IServiceProvider serviceProvider)
        {
            using var _ = Logger.Log.OnEnterAndExit();
            app.UseDotNetInteractiveHttpApi(
                serviceProvider.GetRequiredService<Kernel>(),
                typeof(Program).Assembly,
                serviceProvider.GetRequiredService<HttpProbingSettings>(),
                HttpOptions.HttpPort);
        }
    }
}
