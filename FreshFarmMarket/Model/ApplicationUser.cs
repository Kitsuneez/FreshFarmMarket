using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket.Model
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string CreditCard { get; set; }
		public string? Photo { get; set; }
		public string Address { get; set; }
		public string AboutMe { get; set; }
		public string Gender { get; set; }
	}
}
