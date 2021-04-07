using LibraryApi.Models.Reservations;
using System.Threading.Tasks;

namespace LibraryApi
{
    public interface ILookupReservations
    {
        Task<GetReservationSummaryResponse> GetAllReservationsAsync();
        Task<GetReservationSummaryResponseItem> GetByIdAsync(int id);
    }
}