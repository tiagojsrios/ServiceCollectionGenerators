using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using ServiceCollectionGenerators.Generators;
using ServiceCollectionGenerators.Helpers;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ServiceCollectionGenerators.UnitTests.Helpers;

internal static class TestHelper
{

    public static GeneratorDriverRunResult RunSourceGenerator<T>(string testName) where T : ISourceGenerator
    {
        Compilation inputCompilation = CreateCompilation(EmbeddedResourceHelper
            .GetEmbeddedResource($"{testName}.Input.cs"));

        return CSharpGeneratorDriver.Create(Activator.CreateInstance<T>())
            .RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out _)
            .GetRunResult();
    }

    public static Compilation CreateCompilation(string source)
        => CSharpCompilation.Create("compilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));

    public static Task RunEmbeddedResourceTest(string testName)
    {
        string input = EmbeddedResourceHelper.GetEmbeddedResource($"{testName}.Input.cs");
        string expectedResult = EmbeddedResourceHelper.GetEmbeddedResource($"{testName}.Result.cs");
        
        return new CSharpSourceGeneratorTest<OptionsGenerator, XUnitVerifier>
        {
            TestState =
            {
#if NET6_0
                ReferenceAssemblies = Microsoft.CodeAnalysis.Testing.ReferenceAssemblies.Net.Net60,
#endif
                AdditionalReferences = {
#if NET6_0
                    "Microsoft.Extensions.Hosting.dll",
#endif
                    "Microsoft.Extensions.Configuration.Abstractions.dll",
                    "Microsoft.Extensions.DependencyInjection.Abstractions.dll",
                    "Microsoft.Extensions.Options.dll",
                    "Microsoft.Extensions.Options.DataAnnotations.dll",
                    "Microsoft.Extensions.Options.ConfigurationExtensions.dll"
                },
                Sources = { input },
                GeneratedSources =
                {
                    (typeof(OptionsGenerator), "OptionsAttribute.g.cs", EmbeddedResourceHelper.GetEmbeddedResource(typeof(OptionsGenerator).Assembly, "OptionsAttribute.cs")),
                    (typeof(OptionsGenerator), "ServiceCollectionExtensions.g.cs", expectedResult)
                }
            },
        }.RunAsync();
    }
}