using FluentAssertions;
using Microsoft.CodeAnalysis;
using ServiceCollectionGenerators.Generators;
using ServiceCollectionGenerators.UnitTests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ServiceCollectionGenerators.UnitTests;

public class OptionsGeneratorTests
{
    [Fact]
    public Task DefaultOptionsRegistration() => TestHelper.RunEmbeddedResourceTest(
        nameof(DefaultOptionsRegistration));

    [Fact]
    public Task OptionsCustomSectionNameRegistration() => TestHelper.RunEmbeddedResourceTest(
        nameof(OptionsCustomSectionNameRegistration));

    [Fact]
    public Task OptionsWithoutDataAnnotationsValidation() => TestHelper.RunEmbeddedResourceTest(
        nameof(OptionsWithoutDataAnnotationsValidation));

#if NET6_0
    [Fact]
    public Task OptionsWithValidateOnStartRegistration() => TestHelper.RunEmbeddedResourceTest(
        nameof(OptionsWithValidateOnStartRegistration));

    [Fact]
    public void OptionsWithValidateOnStartButNoDataAnnotationsValidation()
    {
        GeneratorDriverRunResult sourceGeneratorRunResult = TestHelper.RunSourceGenerator<OptionsGenerator>(
            nameof(OptionsWithValidateOnStartButNoDataAnnotationsValidation));
        
        sourceGeneratorRunResult.Diagnostics
            .Should()
            .HaveCount(1);

        sourceGeneratorRunResult.Diagnostics
            .Should()
            .Equal(new List<Diagnostic>
            {
                Diagnostic.Create(
                    new DiagnosticDescriptor("OSG001",
                        "Invalid Configuration",
                        "ValidateDataAnnotations can't be false, when ValidateOnStart is true",
                        nameof(OptionsGenerator), DiagnosticSeverity.Error, isEnabledByDefault: true),
                    Location.None
                )
            });
    }
#endif
}