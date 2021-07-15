using System;
using System.Reflection;
using Funda.Data;
using Funda.Repositories;
using Funda.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System.Threading.Tasks;

namespace Funda
{
    class Program
    {
        private static string BASE_URL = "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f";
        private static string CONNECTION_STRING = "Filename=HouseDB.db";
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            RegisterServices();
            IServiceScope scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<Application>().Run().Wait();
            DisposeServices();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddHttpClient<IHouseFeedClient, HouseFeedClient>(c =>
            {
                c.BaseAddress = new Uri(BASE_URL);
            });
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(CONNECTION_STRING, options =>
             {
                 options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
             }));

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IHouseService, HouseService>();
            services.AddTransient<Application>();

            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
