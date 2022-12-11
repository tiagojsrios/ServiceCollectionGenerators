using ServiceCollectionGenerators.Attributes;

namespace ServiceCollectionGenerators.UnitTests.Resources.ServiceDescriptor
{
    public interface IUserService { }

    [ServiceDescriptor(ServiceLifetime.Transient)]
    public class UserService : IUserService
    {

    }
}