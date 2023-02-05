using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FreshFarmMarket.ViewModels
{
    public class Register
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }
        [Required]
		public string FirstName { get; set; }
		[Required]
        public string LastName { get; set; }
		[Required]
		public string CreditCard { get; set; }
		public string? Photo { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string AboutMe { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        


    }
}
