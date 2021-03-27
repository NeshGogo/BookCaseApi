﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using bookcaseApi.helpers;

namespace bookcaseApi.Entities
{
    public class Author: IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        /*[FirstLetterUpperCase]*/ //Esta es una validacion por atributo.
        public string Name { get; set; }
        public List<Book> Books { get; set; }

        // Asi es como se hace la validacion del modelo completo.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var firstLetter = !string.IsNullOrEmpty(Name) ? Name[0].ToString() : null;

            if (firstLetter != null && firstLetter != firstLetter.ToUpper())
            {
                yield return new ValidationResult("The first letter must be upper case", new string[] { nameof(Name)});
            }
        }
    }
}
