using MedicineMarketPlace.BuildingBlocks.Sms.Models;

namespace MedicineMarketPlace.BuildingBlocks.Sms.Services
{
    public class SmsService : ISmsService
    {
        private readonly SmsConfiguration _smsConfiguration;

        public SmsService(SmsConfiguration smsConfiguration)
        {
            _smsConfiguration = smsConfiguration;
        }

        public async Task<OtpResponse> SendOtp(string mobileNumber, int otp)
        {
            OtpResponse otpResponse = new OtpResponse();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string url = String.Format(_smsConfiguration.ApiUrl, _smsConfiguration.ApiKey, mobileNumber, otp);

                    HttpResponseMessage httpResponse = await httpClient.GetAsync(url);
                    string apiResponse = await httpResponse.Content.ReadAsStringAsync();

                    otpResponse = System.Text.Json.JsonSerializer.Deserialize<OtpResponse>(apiResponse);
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            return otpResponse;
        }
    }
}
