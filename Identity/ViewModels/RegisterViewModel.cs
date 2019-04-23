using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Identity.ViewModels
{
	public class RegisterViewModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }

		public ICommand RegisterCommand
		{
			get
			{
				return new Command(() =>
				{

				});
			}
		}
	}
}
