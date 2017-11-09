using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QCloud.CosApi.Client;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QCloud.CosApi.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please run with file path");
                return;
            }

            var filePath = args[0];

            var conf = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.Development.json", true, true)
                .Build();

            IServiceCollection services = new ServiceCollection();
            services.AddLogging(builder => builder
                .AddConfiguration(conf.GetSection("Logging"))
                .AddConsole());

            services.AddOptions();
            services.Configure<CosClientOptions>(conf.GetSection("cosClient"));
            services.AddSingleton<CosClient>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var cosClient = serviceProvider.GetService<CosClient>();

            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                var result = await cosClient.UploadFileAsync("cnblogsimages2018", "/test/logo.png", fs);
                Console.WriteLine($"{(result ? "Succeeded" : "Failed")} to Upload");
            }
        }
    }
}
