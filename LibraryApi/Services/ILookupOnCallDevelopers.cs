using LibraryApi.Controllers;
using System.Threading.Tasks;

namespace LibraryApi
{
    public interface ILookupOnCallDevelopers
    {
        Task<CachingController.OnCallDeveloperResponse> GetOnCallDeveloperAsync();
    }
}