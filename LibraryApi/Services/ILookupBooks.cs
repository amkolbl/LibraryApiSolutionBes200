using LibraryApi.Models.Books;
using System.Threading.Tasks;

namespace LibraryApi
{
    public interface ILookupBooks
    {
        Task<GetBooksSummaryResponse> GetBooksByGenreAsync(string genre);
        Task<GetBookDetailsResponse> GetBookByIdAsync(int id);
    }
}