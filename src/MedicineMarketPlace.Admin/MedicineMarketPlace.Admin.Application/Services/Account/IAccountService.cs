using MedicineMarketPlace.Admin.Application.Models;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using System.Security.Claims;

namespace MedicineMarketPlace.Admin.Application.Services
{
    public interface IAccountService
    {
        Task<LoginResponseDto> LoginAsync(ApplicationUser user, LoginDto loginDto);
        Task<LoginResponseDto> RefreshTokenAsync(ApplicationUser user, ClaimsPrincipal principal);
    }
}
