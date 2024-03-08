using Microsoft.AspNetCore.Identity;

namespace MedicineMarketPlace.BuildingBlocks.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int? CountryPhoneCode { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiredDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public string FCMToken { get; set; }
        public bool IsStandalone { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
