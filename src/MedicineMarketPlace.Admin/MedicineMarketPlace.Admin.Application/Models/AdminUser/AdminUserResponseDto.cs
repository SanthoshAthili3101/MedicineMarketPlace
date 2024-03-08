namespace MedicineMarketPlace.Admin.Application.Models
{
    public class AdminUserResponseDto
    {
        public int TotalRecords { get; set; }

        public List<AdminUserDto> AdminUsers { get; set; }
    }
}
