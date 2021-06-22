using BooksApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Services
{
    public class StandardHealthChecks : ILookupStatus
    {
        public GetStatusResponse GetMyStatus()
        {
           return new GetStatusResponse
           {
               Message = "Looks Good!",
               LastChecked = DateTime.Now,
               CheckedBy = "joe@aol.com"
           };
        }
    }
}
