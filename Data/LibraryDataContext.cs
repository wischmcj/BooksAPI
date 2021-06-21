using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Data
{
    public class LibraryDataContext : DbContext
    {
        public LibraryDataContext(DbContextOptions<LibraryDataContext> context): base(context)
        {

        }
        public virtual DbSet<Book> Books { get; set; }

        public IQueryable<Book> GetBooksInInventory()
        {
            return Books.Where(b => b.RemovedFromInventory == false);
        }

        public IQueryable<Book> GetBookById(int id)
        {
            return GetBooksInInventory().Where(b => b.Id == id);
        }
    }
}
