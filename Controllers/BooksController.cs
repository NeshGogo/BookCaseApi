using bookcaseApi.Contexts;
using bookcaseApi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookcaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Asi es como indicamos que requerimos la authenticacion por jwt.
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BooksController: ControllerBase
    {
        private readonly BookCaseDbContext _context;

        public BooksController(BookCaseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> Get()
        {
            return _context.Books.Include( b => b.Author).ToList();
        }

        [HttpGet("{id}", Name = "GetBook")]
        public ActionResult<Book> Get(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return NotFound();

            return book;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return new CreatedAtRouteResult("GetBook", new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Book book)
        {
            if (id != book.Id)
                return BadRequest();

            _context.Entry(book).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Book> Delete(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            _context.SaveChanges();
            return book;
        }
    }
}
