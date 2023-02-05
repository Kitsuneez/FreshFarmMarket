using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket.ViewModels
{
	public class Audit
	{
		[Required]
		public string userId { get; set; }
		[Required]
		public string action { get; set; }
		public DateTime timeStamp { get; set; }
	}
}
