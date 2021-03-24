using bookcaseApi.Contexts;
using bookcaseApi.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookcaseApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthorsController: ControllerBase
    {
        private readonly BookCaseDbContext _context;

        public AuthorsController(BookCaseDbContext context)
        {
            _context = context;
        }
        public ActionResult<IEnumerable<Author>> Get()
        {
            return _context.Authors.ToList();
        }
    }
}
