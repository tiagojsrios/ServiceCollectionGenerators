using ServiceCollectionGenerators.Attributes;

namespace ServiceCollectionGenerators.UnitTests.Resources.ServiceDescriptor
{
    public interface IUserService { }

    [ServiceDescriptor(ServiceLifetime.Singleton)]
    public class UserService : IUserService
    {

    }
}