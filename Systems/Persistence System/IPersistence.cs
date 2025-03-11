using System.Threading.Tasks;

namespace SomniaGames.Persistence
{
    public interface IPersistence<T>
    {
        Task SaveAsync(T data);
        Task<T> LoadAsync();
    }
}