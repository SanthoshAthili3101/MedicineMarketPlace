using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineMarketPlace.BuildingBlocks.Firebase.Models
{
    public class DataPayload
    {
        public string title { get; set; }

        public string body { get; set; }

        public string click_action { get; set; }

        public DateTime scheduledDatetime { get; set; }
    }
}
