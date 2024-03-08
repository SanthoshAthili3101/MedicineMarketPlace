namespace MedicineMarketPlace.Admin.Application.Models
{ 
    public class FindAdminUserQueryDto : BaseQueryDto
    {

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
