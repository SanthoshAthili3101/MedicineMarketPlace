namespace MedicineMarketPlace.BuildingBlocks.Firebase.Models
{
    public class ResponseModel
    {
        public string Multicast_id { get; set; }

        public int Success { get; set; }

        public int Failure { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}
