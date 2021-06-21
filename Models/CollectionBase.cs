using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Models
{
    public class CollectionBase<T>
    {
        public List<T> Data { get; set; }
    }
}
