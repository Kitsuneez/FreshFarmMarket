﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshFarmMarket.Model
{
	public class UserHistory
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }
		public string userId { get; set; } = string.Empty;
		public string passwordHash { get; set; } = string.Empty;
	}
}
