using System.Threading.Tasks;
using greengrocer.Models;

namespace greengrocer.Services
{
    public interface IUserService
    {
        Task<User> ValidateUserAsync(string username, string password);
    }
}
