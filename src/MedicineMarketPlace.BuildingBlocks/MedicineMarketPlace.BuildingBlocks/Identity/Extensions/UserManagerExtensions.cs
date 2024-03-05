using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MedicineMarketPlace.BuildingBlocks.Identity.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> FindByEmailFromClaimsPrinciple(this UserManager<ApplicationUser> input, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            return await input.Users.SingleOrDefaultAsync(x => x.Email == email && x.IsActive == true);
        }

        public static async Task<ApplicationUser> FindByUserNameFromClaimsPrinciple(this UserManager<ApplicationUser> input, ClaimsPrincipal user)
        {
            var userName = user.FindFirstValue(ClaimTypes.Name);
            return await input.Users.SingleOrDefaultAsync(x => x.UserName == userName && x.IsActive == true);
        }

        public static async Task<ApplicationUser> FindByIdFromClaimsPrinciple(this UserManager<ApplicationUser> input, ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(nameof(ApplicationUser.Id));
            return await input.Users.SingleOrDefaultAsync(x => x.Id == id);
        }

        public static async Task<List<ApplicationUser>> FindByIdsAsync(this UserManager<ApplicationUser> input, List<string> ids)
        {
            return await input.Users.Where(_ => ids.Contains(_.Id)).ToListAsync();
        }

        public static async Task<List<ApplicationUser>> FindByIdsActiveUsersAsync(this UserManager<ApplicationUser> input, List<string> ids)
        {
            return await input.Users.Where(_ => ids.Contains(_.Id) && _.IsActive == true).ToListAsync();
        }

        public static async Task<ApplicationUser> FindByUserName(this UserManager<ApplicationUser> input, string userName)
        {
            return await input.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public static async Task<ApplicationUser> FindByUserNameStandalone(this UserManager<ApplicationUser> input, string userName)
        {
            return await input.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.IsStandalone == true && x.IsActive == true);
        }

        public static async Task<ApplicationUser> FindByPhoneNumber(this UserManager<ApplicationUser> input, string phoneNumber)
        {
            return await input.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        }

        public static async Task<ApplicationUser> FindByExistingPhoneNumber(this UserManager<ApplicationUser> input, string phoneNumber, string id)
        {
            return await input.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber && x.Id != id);
        }

        public static async Task<ApplicationUser> FindByExistingUserName(this UserManager<ApplicationUser> input, string userName, string id)
        {
            return await input.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.Id != id);
        }

        public static async Task<ApplicationUser> FindByExistingEmail(this UserManager<ApplicationUser> input, string email, string id)
        {
            return await input.Users.FirstOrDefaultAsync(x => x.Email == email && x.Id != id);
        }

        public static async Task<ApplicationUser> FindByEmailStandalone(this UserManager<ApplicationUser> input, string email)
        {
            return await input.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsStandalone == true && x.IsActive == true);
        }

        public static async Task<ApplicationUser> FindInternalByEmailAsync(this UserManager<ApplicationUser> input, string email)
        {
            return await input.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<List<ApplicationUser>> FindUserByEmailAsync(this UserManager<ApplicationUser> input, string email)
        {
            return await input.Users.Where(_ => _.Email == email).ToListAsync(); ;
        }

        public static async Task<ApplicationUser> FindByIdPrinciple(this UserManager<ApplicationUser> input, string id)
        {
            return await input.Users.SingleOrDefaultAsync(x => x.Id == id && x.IsActive == true);
        }

        public static async Task<ApplicationUser> FindByEmailWithoutStandalone(this UserManager<ApplicationUser> input, string email)
        {
            return await input.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsStandalone == false);
        }
    }
}
