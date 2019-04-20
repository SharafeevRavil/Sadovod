using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SadovodBack.Models;
using SadovodBack.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SadovodBack.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	//Контроллер для работы с учетными записями пользователей
	public class AccountController : Controller
	{
		//Сервис по управлению пользователями - UserManager
		private readonly UserManager<User> _userManager;
		//Сервис SignInManager позволяет аутентифицировать пользователя и устанавливать или удалять его куки
		private readonly SignInManager<User> _signInManager;

		//В классе Startup были добавлены сервисы Identity, через конструктор мы можем их получить
		public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		// GET: api/AccountController1
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		// GET: api/AccountController1/5
		[HttpGet]
		public IActionResult Login(string returnUrl = null)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		//Регистрация пользователей
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
				/*Метод _userManager.CreateAsync пользователь добавляется в базу данных.
				 *В качестве параметра передается сам пользователь и его пароль.*/
				var result = await _userManager.CreateAsync(user, model.Password);
				//Метод возвращает объект IdentityResult, с помощью которого можно узнать успешность выполненной операции
				if (result.Succeeded)
				{
					//Установка аутентификационных куки для добавленного пользователя
					/*_signInManager.SignInAsync() устанавливает аутентификационные куки для добавленного пользователя.
					 *В этот метод передается объект пользователя, который аутентифицируется, и логическое значение,
					 *указывающее, надо ли сохранять куки в течение продолжительного времени*/
					await _signInManager.SignInAsync(user, false);
					//Переадресация на главную страницу приложения
					return RedirectToAction("Index", "Home");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						//ModelState добавляет к состоянию модели все возникшие при добавлении ошибки						 
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			//Отправленная модель возвращается в представление
			return View(model);
		}

		// POST: api/AccountController1
		//В Post-версии метода Login получаем данные из представления в виде модели LoginViewModel
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				/*Аутентификацию пользователя выполняет метод signInManager.PasswordSignInAsync(), принимает логин и пароль пользователя,
				 * третий параметр метода указывает, сохранять ли устанавливаемые куки на долгое время				 
				   возвращает IdentityResult, с помощью которого можно узнать, завершилась ли аутентификация успешно*/
				var result =
					await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
				/*Если аутентификация успешна, то используем свойство ReturnUrl модели LoginViewModel
				 * для возврата пользователя на предыдущее место.*/
				if (result.Succeeded)
				{
					//Проверяем, принадлежит ли URL приложению
					if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
					{
						return Redirect(model.ReturnUrl);
					}
					//Если адрес возврата не установлен или не принадлежит приложению, выполняем переадресацию на главную страницу
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", "Неправильный логин и (или) пароль");
				}
			}
			return View(model);
		}

		/*LogOff выполняет выход пользователя из приложения. За выход отвечает метод _signInManager.SignOutAsync(),
		 * который очищает аутентификационные куки*/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogOff()
		{
			// Удаляем аутентификационные куки
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}


		// PUT: api/AccountController1/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE: api/ApiWithActions/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
