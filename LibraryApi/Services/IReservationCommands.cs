using LibraryApi.Models.Reservations;
using System.Threading.Tasks;

namespace LibraryApi
{
    public interface IReservationCommands
    {
        Task<GetReservationSummaryResponseItem> AddReservationAsync(PostReservationRequest request);
        Task<bool> MarkReady(GetReservationSummaryResponseItem item);
        Task<bool> MarkDenied(GetReservationSummaryResponseItem item);
    }
}