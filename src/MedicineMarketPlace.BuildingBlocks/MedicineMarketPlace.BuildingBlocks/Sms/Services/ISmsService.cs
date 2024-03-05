using MedicineMarketPlace.BuildingBlocks.Sms.Models;

namespace MedicineMarketPlace.BuildingBlocks.Sms.Services
{
    public interface ISmsService
    {
        Task<OtpResponse> SendOtp(string mobileNumber, int otp);
    }
}
