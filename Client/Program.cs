using Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace DepthCharts
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // var client = new HttpClient();
            // client.BaseAddress = new Uri("http://localhost:5000");
            // var test = client.GetAsync("/api/getFullDepthChart").Result;
            
            using var host = CreateHostBuilder(args).Build();
            var serviceProvider = host.Services.CreateScope().ServiceProvider;

            var main = serviceProvider.GetRequiredService<Main>();

            main.RunAsync().Wait();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                //configuration.Sources.Clear();

                IHostEnvironment env = hostingContext.HostingEnvironment;
                configuration
                    .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
            })
            .ConfigureLogging((context, config) =>
                config.AddConfiguration(context.Configuration.GetSection("Logging")))
            .ConfigureServices(ConfigureServices);

        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddSingleton<Main, Main>();
            services.AddSingleton<IDepthChart, DepthChart>();

            var serverUrl = hostContext.Configuration.GetSection("ServerUrl").Get<string>();
            if (string.IsNullOrEmpty(serverUrl))
            {
                throw new InvalidOperationException("Please add Server Url in appsettings.json");
            }
            services.AddHttpClient<IDepthChart, DepthChart>(client =>
            {
                client.BaseAddress = new Uri(serverUrl);
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5));
        }
    }
}
