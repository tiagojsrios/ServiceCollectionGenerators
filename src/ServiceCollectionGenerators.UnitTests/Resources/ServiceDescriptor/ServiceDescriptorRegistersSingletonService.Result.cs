using Microsoft.Extensions.DependencyInjection;

namespace ServiceCollectionGenerators.UnitTests.Resources.ServiceDescriptor
{
    public static partial class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services) => services.RegisterServicesForTestProject();
        public static IServiceCollection RegisterServicesForTestProject(this IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            return services;
        }
    }
}
