using bookcaseApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookcaseApi.Contexts
{
    public class BookCaseDbContext: DbContext
    {
        public BookCaseDbContext(DbContextOptions<BookCaseDbContext> options) 
            : base(options)  
        {

        }

        public DbSet<Author> Authors { get; set; }
    }
}
