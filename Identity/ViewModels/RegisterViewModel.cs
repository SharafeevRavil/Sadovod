using Identity.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Identity.ViewModels
{
	public class RegisterViewModel
	{
		APIServices _apiServices = new APIServices();

		public string Email { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public string Message { get; set; }

		public ICommand RegisterCommand
		{
			get
			{
				return new Command(async() =>
				{
					var isSuccess = await _apiServices.RegisterAsync(Email, Password, ConfirmPassword);

					if (isSuccess)
						Message = "Registered Succesfully!";
					else
						Message = "Retry later";
				});
			}
		}
	}
}
