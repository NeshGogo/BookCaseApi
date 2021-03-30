using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookcaseApi.Models
{
    public class ApplicationUser: IdentityUser
    {
        // Esta clase se creo por si queremos extender la informacion que almacenamos del usuario. Cabe destacar que no es obligatoria crearla es opcional.
    }
}
