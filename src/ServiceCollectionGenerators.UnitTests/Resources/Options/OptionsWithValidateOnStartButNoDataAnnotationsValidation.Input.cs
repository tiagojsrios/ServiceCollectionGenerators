using ServiceCollectionGenerators.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Options
{
    [Options(ValidateOnStart = true, ValidateDataAnnotations = false)]
    public class OptionsWithValidateOnStartButNoDataAnnotationsValidation
    {
        [Required] public string ConnectionString { get; set; } = null!;
    }
}
