using System.Threading.Tasks;
using greengrocer.Data;
using greengrocer.Models;
using Microsoft.EntityFrameworkCore;

namespace greengrocer.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public Task<User> ValidateUserAsync(string username, string password)
        {
            
            return _db.Users
                .FirstOrDefaultAsync(u =>
                    u.UserName == username &&
                    u.Password == password &&
                    u.IsActive);
        }
    }
}
