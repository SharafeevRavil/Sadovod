using Identity.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Identity.Services
{
	class APIServices
	{
		public async Task<bool> RegisterAsync(string email, string password, string confirmPassword)
		{
			var client = new HttpClient();

			var model = new RegisterBindingModel
			{
				Email = email,
				Password = password,
				ConfirmPassword = confirmPassword
			};

			var json = JsonConvert.SerializeObject(model);

			HttpContent content = new StringContent(json);

			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var response = await client.PostAsync("http://localhost:52634/Account/Register", content);
			return response.IsSuccessStatusCode;
		}
	}
}
