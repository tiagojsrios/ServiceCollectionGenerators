using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestProject.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterOptions(this IServiceCollection services, IConfiguration configuration) => services.RegisterOptionsForTestProject(configuration);
        public static IServiceCollection RegisterOptionsForTestProject(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<TestProject.Options.OptionsWithValidateOnStartRegistration>().Bind(configuration.GetSection("OptionsWithValidateOnStartRegistration")).ValidateDataAnnotations().ValidateOnStart();
            return services;
        }
    }
}