using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshFarmMarket.Model
{
	public class AuditLog
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }
		public string userId { get; set; }
		public string action { get; set; }
		public DateTime timeStamp { get; set; }
	}
}
