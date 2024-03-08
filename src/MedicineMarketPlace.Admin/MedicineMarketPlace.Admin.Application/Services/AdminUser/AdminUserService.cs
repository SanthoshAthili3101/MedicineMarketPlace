using AutoMapper;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MedicineMarketPlace.BuildingBlocks.Identity.Constants;
using MedicineMarketPlace.Admin.Application.Models;

namespace MedicineMarketPlace.Admin.Application.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AdminUserService(
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<Tuple<bool, string>> CreateAsync(CreateUserDto dto, ApplicationUser user)
        {
            var userToBeCreated = _mapper.Map<ApplicationUser>(dto);
            userToBeCreated.CreatedBy = user.Email;
            userToBeCreated.CreatedDate = DateTime.UtcNow;

            try
            {
                var userResponse = await _userManager.CreateAsync(userToBeCreated, dto.Password);
                if (!userResponse.Succeeded)
                    return Tuple.Create(false, userResponse.ToString());

                await _userManager.AddToRoleAsync(userToBeCreated, dto.Role);
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(userToBeCreated);
                return Tuple.Create(false, string.Empty);
            }

            return Tuple.Create(true, string.Empty);
        }

        public async Task<Tuple<bool, string>> UpdateAsync(string id, UpdateUserDto dto, ApplicationUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(id);

            _mapper.Map(dto, existingUser);
            existingUser.ModifiedBy = user.Email;
            existingUser.ModifiedDate = DateTime.UtcNow;
            try
            {
                var userResponse = await _userManager.UpdateAsync(existingUser);
                if (!userResponse.Succeeded)
                    return Tuple.Create(false, userResponse.ToString());

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, string.Empty);
            }
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAdminUsersAsync()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync(ApplicationUserRoles.Admin);
            return adminUsers;
        }
        
    }
}
