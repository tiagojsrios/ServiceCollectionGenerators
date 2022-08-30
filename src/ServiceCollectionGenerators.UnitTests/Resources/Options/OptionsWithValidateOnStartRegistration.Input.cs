using ServiceCollectionGenerators.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Options
{
    [Options(ValidateOnStart = true)]
    public class OptionsWithValidateOnStartRegistration
    {
        [Required] public string ConnectionString { get; set; } = null!;
    }
}
