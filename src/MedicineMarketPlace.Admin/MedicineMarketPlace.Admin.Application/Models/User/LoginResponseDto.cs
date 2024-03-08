namespace MedicineMarketPlace.Admin.Application.Models
{
    public class LoginResponseDto
    {
        public LoginUserDto User { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
