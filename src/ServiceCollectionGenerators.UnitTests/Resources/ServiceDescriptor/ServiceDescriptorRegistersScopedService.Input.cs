using ServiceCollectionGenerators.Attributes;

namespace ServiceCollectionGenerators.UnitTests.Resources.ServiceDescriptor
{
    public interface IUserService { }

    [ServiceDescriptor(ServiceLifetime.Scoped)]
    public class UserService : IUserService
    {

    }
}