using ServiceCollectionGenerators.UnitTests.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace ServiceCollectionGenerators.UnitTests;

public class ServiceDescriptorGeneratorTests
{
    [Fact]
    public Task ServiceDescriptorRegistersSingletonService() => TestHelper.RunEmbeddedResourceTest(
        nameof(ServiceDescriptorRegistersSingletonService));

    [Fact]
    public Task ServiceDescriptorRegistersScopedService() => TestHelper.RunEmbeddedResourceTest(
        nameof(ServiceDescriptorRegistersScopedService));

    [Fact]
    public Task ServiceDescriptorRegistersTransientService() => TestHelper.RunEmbeddedResourceTest(
        nameof(ServiceDescriptorRegistersTransientService));
}