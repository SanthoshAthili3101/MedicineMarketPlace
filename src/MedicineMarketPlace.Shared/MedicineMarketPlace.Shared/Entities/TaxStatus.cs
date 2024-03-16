using MedicineMarketPlace.BuildingBlocks.EntityFramework.Entities;

namespace MedicineMarketPlace.Shared.Entities
{
    public class TaxStatus : Entity<int>
    {
        public const string TaxStatusTable = "TaxStatus";

        public string Name { get; set; }
    }
}
