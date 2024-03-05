using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineMarketPlace.BuildingBlocks.Api.Models
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null, object details = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            Data = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorised, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side. Server errors lead to anger",
                _ => null
            };
        }
    }
}
