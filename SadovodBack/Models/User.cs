using Microsoft.AspNetCore.Identity;
namespace SadovodBack.Models
{
	public class User : IdentityUser
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string UserName { get; set; }

		public string Gender { get; set; }
	}
}
