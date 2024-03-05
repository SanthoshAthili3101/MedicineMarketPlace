using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineMarketPlace.BuildingBlocks.Firebase.Models
{
    public class NotificationModel
    {
        public List<string> registration_ids { get; set; }

        public DataPayload notification { get; set; }

        public string priority { get; set; } = "high";
    }
}
