using Microsoft.AspNetCore.Identity;
using ProductManagement.Models.ViewModel;
using System.Threading.Tasks;

namespace ProductManagement.Services
{
    public interface IUserService
    {
        Task<bool> LoginAsync(LoginViewModel model);
        Task<bool> RegisterAsync(RegisterViewModel model);
        Task<IList<string>> GetRolesAsync(string email);
        Task<IdentityUser> FindByEmailAsync(string email);
        Task LogoutAsync();

    }

}
