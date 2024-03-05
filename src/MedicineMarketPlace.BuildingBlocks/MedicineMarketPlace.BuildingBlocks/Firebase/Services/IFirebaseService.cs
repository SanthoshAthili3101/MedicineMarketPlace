using MedicineMarketPlace.BuildingBlocks.Firebase.Models;

namespace MedicineMarketPlace.BuildingBlocks.Firebase.Services
{
    public interface IFirebaseService
    {
        Task<ResponseModel> SendNotificationAsync(List<string> tokens, string title, string body, string action);
    }
}
