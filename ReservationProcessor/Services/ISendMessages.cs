using ReservationProcessor.Models;
using System.Threading.Tasks;

namespace ReservationProcessor.Services
{
    public interface ISendMessages
    {
        Task WriteFailedReservation(BookReservationResponse message);
        Task WriteSuccessfulReservation(BookReservationResponse message);
    }
}