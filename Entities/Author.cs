using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using bookcaseApi.helpers;

namespace bookcaseApi.Entities
{
    public class Author 
    {
        public int Id { get; set; }
        [Required]
        //Esta es una validacion por atributo custom.
        [FirstLetterUpperCase] 
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Book> Books { get; set; }

        // Asi es como se hace la validacion del modelo completo. Recordar que hay que utilizar la interfaz IValidatableObject.
        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var firstLetter = !string.IsNullOrEmpty(Name) ? Name[0].ToString() : null;

            if (firstLetter != null && firstLetter != firstLetter.ToUpper())
            {
                yield return new ValidationResult("The first letter must be upper case", new string[] { nameof(Name)});
            }
        }*/
    }
}
