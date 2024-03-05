using MedicineMarketPlace.BuildingBlocks.Firebase.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MedicineMarketPlace.BuildingBlocks.Firebase.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseConfiguration _firebaseConfiguration;

        public FirebaseService(FirebaseConfiguration firebaseConfiguration)
        {
            _firebaseConfiguration = firebaseConfiguration;
        }

        public async Task<ResponseModel> SendNotificationAsync(List<string> tokens, string title, string body, string action)
        {
            using (var client = new HttpClient())
            {
                var response = new ResponseModel();
                client.BaseAddress = new Uri(_firebaseConfiguration.ApiUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
                    $"key={_firebaseConfiguration.ServerKey}");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Sender", $"id={_firebaseConfiguration.SenderId}");

                var notification = new NotificationModel()
                {
                    registration_ids = tokens,
                    notification = new DataPayload()
                    {
                        title = title,
                        body = body,
                        click_action = action,
                        scheduledDatetime = DateTime.Now.AddSeconds(2)
                    }
                };

                var json = JsonConvert.SerializeObject(notification);
                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse = await client.PostAsync("send", requestContent);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ResponseModel>(content);
                    response.IsSuccess = true;
                    response.Message = "Notification sent successfully";
                }
                else
                {
                    var errorMessage = JsonConvert.DeserializeObject<string>(await httpResponse.Content.ReadAsStringAsync());
                    response.IsSuccess = false;
                    response.Message = errorMessage;
                }

                return response;
            }
        }
    }
}
