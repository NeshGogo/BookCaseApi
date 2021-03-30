using bookcaseApi.Entities;
using bookcaseApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookcaseApi.Contexts
{
    public class BookCaseDbContext: IdentityDbContext<ApplicationUser>
    {
        public BookCaseDbContext(DbContextOptions<BookCaseDbContext> options) 
            : base(options)  
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
