using ServiceCollectionGenerators.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCollectionGenerators.Models;

public class OptionsRegistrations
{
    public string Name { get; }

    public string ConfigurationSectionName { get; }

    public bool ValidateDataAnnotations { get; }

    public bool ValidateOnStart { get; }

    public OptionsRegistrations(string name, string configurationSectionName, bool validateDataAnnotations, bool validateOnStart)
    {
        Name = name;
        ConfigurationSectionName = configurationSectionName;
        ValidateDataAnnotations = validateDataAnnotations;
        ValidateOnStart = validateOnStart;
    }

    public static string RenderClass(string @namespace, IEnumerable<OptionsRegistrations> options) =>
        $@"using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace {@namespace}.Extensions
{{
    public static partial class ServiceCollectionExtensions
    {{
        internal static IServiceCollection RegisterOptions(this IServiceCollection services, IConfiguration configuration) => services.RegisterOptionsFor{@namespace.ToSafeIdentifier()}(configuration);
        
        public static IServiceCollection RegisterOptionsFor{@namespace.ToSafeIdentifier()}(this IServiceCollection services, IConfiguration configuration)
        {{
            {string.Join("\n", options.Select(GetDependencyInjectionEntry))}

            return services;
        }}
    }}
}}";
    /// <summary>
    ///     Creates the string to be appended to the generated <see cref="IServiceCollection"/> extension method
    /// </summary>
    public static string GetDependencyInjectionEntry(OptionsRegistrations options)
    {
        StringBuilder sb = new($"services.AddOptions<{options.Name}>()" +
                   $".Bind(configuration.GetSection(\"{options.ConfigurationSectionName}\"))");
        
        if (options.ValidateDataAnnotations)
        {
            sb.Append(".ValidateDataAnnotations()");
        }
        
        if (options.ValidateOnStart)
        {
            sb.Append(".ValidateOnStart()");
        }

        sb.Append(";");
        return sb.ToString();
    }
}