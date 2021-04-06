using System.Threading.Tasks;

namespace LibraryApi
{
    public interface IBookCommands
    {
        Task RemoveBookAsync(int id);
    }
}