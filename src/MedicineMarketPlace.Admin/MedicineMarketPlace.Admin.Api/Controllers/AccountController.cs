using MedicineMarketPlace.Admin.Domain.Constants;
using MedicineMarketPlace.BuildingBlocks.Api.Controllers;
using MedicineMarketPlace.BuildingBlocks.Api.Models;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using MedicineMarketPlace.BuildingBlocks.Identity.Extensions;
using MedicineMarketPlace.BuildingBlocks.Identity.Services.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using MedicineMarketPlace.Admin.Application.Services;
using MedicineMarketPlace.Admin.Application.Models;

namespace MedicineMarketPlace.Admin.Api.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _tokenService;
        private readonly IAccountService _accountService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService tokenService,
            IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _accountService = accountService;
        }

        /// <summary>
        /// Retrive the logged in user details.
        /// </summary>
        /// <param name="loginDto"> Pass the user login details. </param>     
        /// <returns>A <see cref="IActionResult"/> representing the asynchronous operation.</returns>
        [HttpPost("Login")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(LoginResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail) ?? await _userManager.FindByUserName(loginDto.UserNameOrEmail);
            if (user == null)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, CommonMessages.UserNotFound));

            var roles = await _userManager.GetRolesAsync(user);
            // This i need to uncomment once i implemented role for customer
            /*if (!roles.Any(r => r == ApplicationUserRoles.User))
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded)
                    return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, CommonMessages.IncorrectPassword));
            }*/

            var response = await _accountService.LoginAsync(user, loginDto);
            return Ok(new ApiResponse((int)HttpStatusCode.OK, CommonMessages.Login, response));
        }

    }
}
