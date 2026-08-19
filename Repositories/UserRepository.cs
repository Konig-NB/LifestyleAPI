using LifestyleAPI.Data;
using LifestyleAPI.Models;
using LifestyleAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LifestyleAPI.Repositories
{
    public class UserRepository : Repository<User> ,IUserRepository
    {
        public UserRepository(AppDbContext db) : base(db) {}

        public async Task<IEnumerable<User>> GetAllUsersAsync(int page, int pagesize) =>
            await _db.Users
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

        public async Task<int> GetTotalCountAsync() =>
            await _db.Users.CountAsync();

        public async Task<User?> GetByIdUserAsync(int id) =>
            await _db.Users
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<bool> IsPhoneNumberTakenAsync(string phoneNumber, int? excludeId = null)
        {
            var query = _db.Users.Where(b => b.PhoneNumber == phoneNumber);

            if(excludeId.HasValue)
                query = query.Where(b => b.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}