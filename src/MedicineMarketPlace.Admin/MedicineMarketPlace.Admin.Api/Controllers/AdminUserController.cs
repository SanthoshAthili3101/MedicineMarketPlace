using MedicineMarketPlace.Admin.Application.Services;
using MedicineMarketPlace.Admin.Domain.Constants;
using MedicineMarketPlace.BuildingBlocks.Api.Controllers;
using MedicineMarketPlace.BuildingBlocks.Api.Models;
using MedicineMarketPlace.BuildingBlocks.Identity.Constants;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using MedicineMarketPlace.BuildingBlocks.Identity.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using MedicineMarketPlace.Admin.Application.Models;

namespace MedicineMarketPlace.Admin.Api.Controllers
{
    [Authorize(Policy = ApplicationUserPolicies.SuperAdminPolicy)]
    public class AdminUserController : ApiControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminUserService _adminService;
        public AdminUserController(
              UserManager<ApplicationUser> userManager,
              IAdminUserService adminService)
        {
            _userManager = userManager;
            _adminService = adminService;
        }

        /// <summary>
        /// Get All the Admin User details.
        /// </summary>             
        /// <returns>A <see cref="IActionResult"/> representing the asynchronous operation.</returns>
        [HttpGet("~/GetAllAdminUsers")]
        [ProducesResponseType(typeof(IEnumerable<ApplicationUser>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAdminUsers()
        {
            var users = await _adminService.GetAllAdminUsersAsync();
            if (users == null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound, CommonMessages.UsersNotFound));
            return Ok(new ApiResponse((int)HttpStatusCode.OK, CommonMessages.AllAdminUsers, users));
        }

        /// <summary>
        ///  /// Get the User by Id.
        /// </summary>        
        /// <param name="dto"> Pass the user id. </param>        
        /// <returns>A <see cref="IActionResult"/> representing the asynchronous operation.</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ApplicationUser), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _adminService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound, CommonMessages.UserNotFound));

            return Ok(new ApiResponse((int)HttpStatusCode.OK, CommonMessages.GetUserById, user));
        }

        /// <summary>
        /// Add the admin and user details.
        /// </summary>        
        /// <param name="dto"> Pass the admin or user details. </param>        
        /// <returns>A <see cref="IActionResult"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateUserDto dto)
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (user == null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound, CommonMessages.UserNotFound));

            var existingUserName = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUserName != null)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, CommonMessages.UserNameIsInUse));

            var existingEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingEmail != null)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, CommonMessages.EmailIsInUse));

            // This i need to uncomment once i implemented role for customer

            //if (dto.Role == ApplicationUserRoles.User.ToString())
            //    return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, CommonMessages.OnlyAdminRoleCreate));

            var response = await _adminService.CreateAsync(dto, user);
            if (response.Item2 != string.Empty)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, response.Item2));

            if (response.Item1 == false)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, CommonMessages.SomethingWentWrong));

            return Ok(new ApiResponse((int)HttpStatusCode.OK, CommonMessages.CreateUser));
        }

        /// <summary>
        /// Update the user details.
        /// </summary> 
        /// <param name="id">Pass the user id. </param>        
        /// <param name="dto"> Pass the user details. </param>        
        /// <returns>A <see cref="IActionResult"/> representing the asynchronous operation.</returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] UpdateUserDto dto)
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (user == null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound, CommonMessages.UserNotFound));

            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound, CommonMessages.UserNotFound));

            var existingEmail = await _userManager.FindByExistingEmail(dto.Email, id);
            if (existingEmail != null)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, CommonMessages.EmailIsInUse));
            var response = await _adminService.UpdateAsync(id, dto, user);
            if (response.Item2 != string.Empty)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, response.Item2));

            if (response.Item1 == false)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, CommonMessages.SomethingWentWrong));

            return Ok(new ApiResponse((int)HttpStatusCode.OK, CommonMessages.UpdateUser));
        }

        /// <summary>
        ///  /// delete the Admin user details.
        /// </summary>        
        /// <param name="dto"> Pass the user id. </param>        
        /// <returns>A <see cref="IActionResult"/> representing the asynchronous operation.</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (user == null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound, CommonMessages.UserNotFound));

            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound, CommonMessages.UserNotFound));

            var result = await _userManager.DeleteAsync(existingUser);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, CommonMessages.SomethingWentWrong));

            return Ok(new ApiResponse((int)HttpStatusCode.OK, CommonMessages.DeleteAdminUser));
        }

    }
}
