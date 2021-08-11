using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string[] txtList = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Logs"), "*.txt");
            foreach (var file in txtList)
            {
                File.Delete(file);
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
