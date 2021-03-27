using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bookcaseApi.helpers
{
    public class FirstLetterUpperCaseAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Avoid validate others responsibilities.
            if(value == null)
            {
                return ValidationResult.Success;
            }

            var firstLetter = value.ToString()[0].ToString();
            
            if(firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("The first letter must be upper case");
            }

            return ValidationResult.Success;
        }
    }
}
