using System.ComponentModel.DataAnnotations;

namespace BankingAPI.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(60)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(60)]
        public string LastName { get; set; }

        [Required]
        [StringLength(11)]
        public string PersonalId { get; set; }

        public string ProfilePhoto { get; set; }

        [Required]
        [PhoneNumber("SA")]
        public string MobileNumber { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        [MinAccountCount(1, ErrorMessage = "At least one account is required.")]
        public Address Address { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }

    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
    }

    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string AccountNumber { get; set; }
    }
}
