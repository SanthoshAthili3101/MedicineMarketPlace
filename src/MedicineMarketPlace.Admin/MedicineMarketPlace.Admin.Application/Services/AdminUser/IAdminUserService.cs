using MedicineMarketPlace.Admin.Application.Models;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;

namespace MedicineMarketPlace.Admin.Application.Services
{
    public interface IAdminUserService
    {

        Task<Tuple<bool, string>> CreateAsync(CreateUserDto dto, ApplicationUser user);

        Task<Tuple<bool, string>> UpdateAsync(string id, UpdateUserDto dto, ApplicationUser user);

        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> GetAllAdminUsersAsync();
    }
}
