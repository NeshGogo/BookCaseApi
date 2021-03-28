using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bookcaseApi.Models
{
    public class AuthorCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
