using AutoMapper;
using bookcaseApi.Contexts;
using bookcaseApi.Entities;
using bookcaseApi.helpers;
using bookcaseApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public AuthorsController(BookCaseDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("CurrentSecond")]
        [ResponseCache(Duration = 15)]
        [ServiceFilter(typeof(CustomFilterToAction))]
        public ActionResult<string> CurrentSecond()
        {
            return DateTime.Now.Second.ToString();
        }

        [HttpGet("List")]
        [Authorize]
        public ActionResult<IEnumerable<Author>> List()
        {
            return _context.Authors.Include(a => a.Books).ToList();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> Get()
        {
            var authors = await _context.Authors.Include(a => a.Books).ToListAsync();
            var authorsDTO = _mapper.Map<List<AuthorDTO>>(authors);
            return authorsDTO;
        }

        [HttpGet("{id}", Name = "GetAuthor")]
        public async Task<ActionResult<AuthorDTO>> Get(int id )
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
            
            if(author == null)          
                return NotFound();
            var authorDTO = _mapper.Map<AuthorDTO>(author);
            return authorDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AuthorCreateDTO authorCreateDTO)
        {
            var author = _mapper.Map<Author>(authorCreateDTO);
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            var authorDTO = _mapper.Map<AuthorDTO>(author);
            return new CreatedAtRouteResult("GetAuthor", new { id = author.Id }, authorDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id,[FromBody] AuthorCreateDTO authorUpdateDTO)
        {
            var author = _mapper.Map<Author>(authorUpdateDTO);
            author.Id = id;
            _context.Entry(author).State = EntityState.Modified;
            /*_context.Authors.Update(author);*/ // Actualizaciones parciales.
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<AuthorCreateDTO> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();
            
            var authorDb = await _context.Authors.FindAsync(id);
            
            if (authorDb == null)
                return NotFound();
           
            var authorDTO = _mapper.Map<AuthorCreateDTO>(authorDb);

            patchDocument.ApplyTo(authorDTO, ModelState);

            _mapper.Map(authorDTO, authorDb);

            if (!TryValidateModel(authorDb))
                return BadRequest(ModelState);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>> Delete(int id)
        {
            var authorId = await _context.Authors.Select(author => author.Id).FirstOrDefaultAsync(Id => Id == id);
            
            if (authorId  == default(int))
                return NotFound();

            _context.Authors.Remove(new Author { Id = authorId});
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
