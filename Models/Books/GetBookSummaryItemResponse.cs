using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Models.Books
{
    public class GetBookSummaryItemResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }

}
