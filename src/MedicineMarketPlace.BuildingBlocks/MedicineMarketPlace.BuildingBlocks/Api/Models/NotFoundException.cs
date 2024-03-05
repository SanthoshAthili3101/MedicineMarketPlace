using System.Net;

namespace MedicineMarketPlace.BuildingBlocks.Api.Models
{
    public class NotFoundException : Exception
    {
        public HttpStatusCode statusCode { get; private set; }

        public NotFoundException(string message, HttpStatusCode statusCode) : base(message)
        {
            this.statusCode = statusCode;
        }
    }
}
