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

            var bucketName = "cnblogsimages2018";
            var remotePath = "/test/logo.png";
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                var result = await cosClient.UploadFile(bucketName, "/test/logo.png", fs);
                if (result.Success)
                {
                    Console.WriteLine("Succeeded to upload");
                    Console.WriteLine("access url: " + result.Value);

                    result = await cosClient.DeleteFile(bucketName, remotePath);
                    Console.WriteLine($"{(result.Success ? "Succeeded" : "Failed")} to delete");
                }
                else
                {
                    Console.WriteLine("Failed to upload");
                }
            }
        }
    }
}
