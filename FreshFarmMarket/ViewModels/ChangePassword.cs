﻿using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket.ViewModels
{
	public class ChangePassword
	{
		[Required]
		public string userId { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
		public DateTime ConfirmPassword { get; set; }
	}
}
