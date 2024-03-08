using AutoMapper;
using MedicineMarketPlace.Admin.Application.Models;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using MedicineMarketPlace.BuildingBlocks.Identity.Services.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MedicineMarketPlace.Admin.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _tokenService;
        private readonly IMapper _mapper;
        private readonly JwtConfiguration _configuration;
        public AccountService(
               UserManager<ApplicationUser> userManager,
               IJwtService tokenService,
               IMapper mapper,
               JwtConfiguration configuration)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(ApplicationUser user, LoginDto loginDto)
        {
            var response = new LoginResponseDto();
            var claims = new List<Claim>
            {
                new Claim(nameof(ApplicationUser.Id), user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var refreshToken = _tokenService.CreateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiredDate = DateTime.UtcNow.AddMinutes(_configuration.RefreshTokenExpiry);
            await _userManager.UpdateAsync(user);

            var token = _tokenService.CreateToken(claims, _configuration.AccessTokenExpiry);

            response.User = _mapper.Map<LoginUserDto>(user);
            response.User.Role = roles.FirstOrDefault();
            response.Token = token;
            response.RefreshToken = refreshToken;

            return response;
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(ApplicationUser user, ClaimsPrincipal principal)
        {
            var refreshToken = _tokenService.CreateRefreshToken();
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return new LoginResponseDto()
            {
                Token = _tokenService.CreateToken(principal.Claims.ToList(), _configuration.AccessTokenExpiry),
                User = _mapper.Map<LoginUserDto>(user),
                RefreshToken = refreshToken
            };
        }
    }
}
