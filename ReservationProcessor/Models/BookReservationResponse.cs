using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationProcessor.Models
{
    public class BookReservationResponse
    {
        public string ReservationId { get; set; }
        public string For { get; set; }
        public List<Book> Books { get; set; }
        public string Status { get; set; }
    }
}
