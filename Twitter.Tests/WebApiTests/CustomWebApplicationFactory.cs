using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using Twitter.Persistence;
using Twitter.WebApi;

namespace Twitter.Tests.WebApiTests
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RemoveLibraryDbContextRegistration(services);

                var serviceProvider = GetInMemoryServiceProvider();

                services.AddDbContextPool<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase(Guid.Empty.ToString());
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var dir = Path.GetDirectoryName("D:\\Asp.Net(project)\\Twitter\\Twitter.WebApi\\Resources");
                Environment.CurrentDirectory = dir;

                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                    UnitTestHelper.SeedData(context);
                }
            });
        }

        private static ServiceProvider GetInMemoryServiceProvider()
        {
            return new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
        }

        private static void RemoveLibraryDbContextRegistration(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }
    }
}
