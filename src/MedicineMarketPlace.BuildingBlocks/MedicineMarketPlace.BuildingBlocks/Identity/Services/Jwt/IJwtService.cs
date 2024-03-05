using System.Security.Claims;

namespace MedicineMarketPlace.BuildingBlocks.Identity.Services.Jwt
{
    public interface IJwtService
    {
        string CreateToken(List<Claim> claims, int duration);

        string CreateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
