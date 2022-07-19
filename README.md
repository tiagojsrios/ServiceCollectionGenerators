# ServiceCollectionGenerators

## OptionsAttribute
Another common practice while developing .NET applications is to create binding classes. This allows you to configure object properties' values in `appsettings.json`, create and add an object to the dependency injection container as a `IOptions<T>`.

```csharp
namespace My.Namespace;

[Options]
public class DatabaseContextOptions
{
    [Required] public string ConnectionString { get; set; }
}
```

This will generate the following code:

```csharp
namespace My.Namespace.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterOptions(this IServiceCollection services) => services.RegisterOptionsForMyNamespace();
        
        public static IServiceCollection RegisterOptionsForMyNamespace(this IServiceCollection services)
        {
            services.AddOptions<My.Namespace.DatabaseContextOptions>().Bind(configuration.GetSection("DatabaseContextOptions")).ValidateDataAnnotations();
            return services;
        }
    }
}
```

The generated code will make use of the `.ValidateDataAnnotations()` method. If needed, you can change this behaviour by set the attribute property `ValidateDataAnnotations` to false.

The attribute also allows you to change the Configuration Section name that will be used to bind the Options from the IConfiguration object.
