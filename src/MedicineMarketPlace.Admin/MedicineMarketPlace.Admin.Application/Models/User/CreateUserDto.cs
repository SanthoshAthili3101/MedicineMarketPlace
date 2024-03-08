namespace MedicineMarketPlace.Admin.Application.Models
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int CountryPhoneCode { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Pincode { get; set; }

        public string City { get; set; }

        public int? StateId { get; set; }

        public int? CountryId { get; set; }

        public string Role { get; set; }
    }
}
