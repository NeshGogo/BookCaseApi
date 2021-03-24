﻿using bookcaseApi.Contexts;
using bookcaseApi.Entities;
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

        public AuthorsController(BookCaseDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Author>> Get()
        {
            return _context.Authors.ToList();
        }
        [HttpGet("{id}", Name = "GetAuthor")]
        public ActionResult<Author> Get(int id )
        {
            var author = _context.Authors.FirstOrDefault(a => a.Id == id);
            
            if(author == null)          
                return NotFound();
           
            return author;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Author author)
        {
            _context.Add(author);
            _context.SaveChanges();
            return new CreatedAtRouteResult("GetAuthor", new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id,[FromBody] Author author)
        {
            if (id != author.Id)
                return BadRequest();

            _context.Entry(author).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public ActionResult<Author> Delete(int id)
        {
            var author = _context.Authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
                return NotFound();

            _context.Authors.Remove(author);
            _context.SaveChanges();
            return author;
        }
    }
}
