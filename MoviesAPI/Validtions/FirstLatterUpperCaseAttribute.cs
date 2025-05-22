using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Validtions
{
    public class FirstLatterUpperCaseAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString())) {
                return ValidationResult.Success;
            }

            var firstLatter = value.ToString()![0].ToString();

            if (firstLatter != firstLatter.ToUpper()) {
                return new ValidationResult("the first letter must be upper case");
            }

            return ValidationResult.Success;
        }
    }
}
