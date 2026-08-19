using LifestyleAPI.Models;

namespace LifestyleAPI.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetAllUsersAsync(int page, int pageSize);
        Task<User?> GetByIdUserAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<bool> IsPhoneNumberTakenAsync(string phoneNumber, int? excludedId = null);
    }
}