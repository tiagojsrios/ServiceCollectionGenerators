﻿using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using ServiceCollectionGenerators.Generators;
using ServiceCollectionGenerators.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ServiceCollectionGenerators.UnitTests;

public class OptionsGeneratorTests
{
    [Fact]
    public Task DefaultOptionsRegistration() => RunEmbeddedResourceTest(nameof(DefaultOptionsRegistration));

    [Fact]
    public Task OptionsCustomSectionNameRegistration() => RunEmbeddedResourceTest(nameof(OptionsCustomSectionNameRegistration));

    [Fact]
    public Task OptionsWithoutDataAnnotationsValidation() => RunEmbeddedResourceTest(nameof(OptionsWithoutDataAnnotationsValidation));

    [Fact]
    public Task OptionsWithValidateOnStartRegistration() => RunEmbeddedResourceTest(nameof(OptionsWithValidateOnStartRegistration));

    private static Task RunEmbeddedResourceTest(string testName, IDictionary<string, string>? placeholders = null)
    {
        string input = EmbeddedResourceHelper.GetEmbeddedResource($"{testName}.Input.cs");
        string expectedResult = EmbeddedResourceHelper.GetEmbeddedResource($"{testName}.Result.cs");

        foreach ((string key, string value) in placeholders ?? Enumerable.Empty<KeyValuePair<string, string>>())
        {
            input = input.Replace(key, value);
            expectedResult = expectedResult.Replace(key, value);
        }

        return new CSharpSourceGeneratorTest<OptionsGenerator, XUnitVerifier>
        {
            TestState =
            {
                AdditionalReferences = {
                    "Microsoft.Extensions.Configuration.Abstractions.dll",
                    "Microsoft.Extensions.DependencyInjection.Abstractions.dll",
                    "Microsoft.Extensions.Options.dll",
                    "Microsoft.Extensions.Options.DataAnnotations.dll",
                    "Microsoft.Extensions.Options.ConfigurationExtensions.dll",
                    "Microsoft.Extensions.Hosting.dll"
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