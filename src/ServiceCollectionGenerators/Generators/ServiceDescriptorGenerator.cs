using Microsoft.CodeAnalysis;
using ServiceCollectionGenerators.Helpers;

namespace ServiceCollectionGenerators.Generators;

[Generator]
public class ServiceDescriptorGenerator : ISourceGenerator
{
    public const string ServiceDescriptorAttribute = nameof(ServiceDescriptorAttribute);

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(ctx => ctx.AddSource($"{ServiceDescriptorAttribute}.g.cs", 
            EmbeddedResourceHelper.GetEmbeddedResource($"{ServiceDescriptorAttribute}.cs")));
        context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver(false, ServiceDescriptorAttribute));
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        throw new System.NotImplementedException();
    }
}