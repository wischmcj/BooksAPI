using BooksApi.Controllers;

namespace BooksApi.Services
{
    public interface ILookupStatus
    {
        GetStatusResponse GetMyStatus();
    }
}