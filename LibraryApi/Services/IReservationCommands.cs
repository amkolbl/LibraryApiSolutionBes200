using LibraryApi.Models.Reservations;
using System.Threading.Tasks;

namespace LibraryApi
{
    public interface IReservationCommands
    {
        Task<GetReservationSummaryResponseItem> AddReservationAsync(PostReservationRequest request);
    }
}