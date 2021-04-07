using LibraryApi.Domain;
using LibraryApi.Models.Reservations;
using System.Threading.Tasks;

namespace LibraryApi
{
    public interface IProcessReservations
    {
        Task AddWorkAsync(BookReservation reservation);
    }
}