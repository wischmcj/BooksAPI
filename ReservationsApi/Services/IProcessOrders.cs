using ReservationsApi.Controllers;
using System.Threading.Tasks;

namespace ReservationsApi.Services
{
    public interface IProcessOrders
    {
        Task Send(ReservationResponse response);
    }
}