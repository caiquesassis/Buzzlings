using Buzzlings.BusinessLogic.Services.Buzzling;
using Buzzlings.BusinessLogic.Services.Hive;
using Buzzlings.BusinessLogic.Services.Simulation;
using Buzzlings.BusinessLogic.Services.TopHive;
using Buzzlings.BusinessLogic.Services.User;

//We need to use this namespace so the extension methods show up in Program.cs without "using"
namespace Microsoft.Extensions.DependencyInjection
{
    public static class BusinessRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHiveService, HiveService>();
            services.AddScoped<IBuzzlingService, BuzzlingService>();
            services.AddScoped<ITopHiveService, TopHiveService>();
            services.AddScoped<ISimulationService, SimulationService>();

            return services;
        }
    }
}