using Buzzlings.Data.Contexts;
using Buzzlings.Data.Repositories;
using Buzzlings.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

//We need to use this namespace so the extension methods show up in Program.cs without "using"
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataRegistration
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IHiveRepository, HiveRepository>();
            services.AddScoped<IBuzzlingRepository, BuzzlingRepository>();
            services.AddScoped<IBuzzlingRoleRepository, BuzzlingRoleRepository>();
            services.AddScoped<ITopHiveRepository, TopHiveRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}