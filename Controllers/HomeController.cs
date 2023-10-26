using LerningAsp.Entyties;
using LerningAsp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LerningAsp.Controllers
{
    
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)                           
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}
      
        public IActionResult About()
		{
			return View();
		}

		[HttpPost]
		public  IActionResult CheckRegistr(Registr reg)
		{
			
			if (ModelState.IsValid)
			{
				using (Context db = new Context())
				{
					var persona = new User() { Login = reg.Login, Email = reg.EMail, Password = reg.Password };
					db.Users.Add(persona);
                    db.SaveChanges();
					
                }
                    return Redirect("/");
			}
			else
			{
				return View("Registr");
            }
				
		}
        public IActionResult Registr()
        {
           
            return View();
        }

		public IActionResult Loging()
		{
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckLogging(Logging log)
        {
            
            if (ModelState.IsValid)
            {
                using (Context db = new Context())
                {
					var allusers = db.Users.ToList();

                    for(int i = 0; i < allusers.Count; i++)
					{
						if (allusers[i].Login == log.Login)
                        {
							if (allusers[i].Password == log.Password)
							{
                                await Authenticate(log.Login); // аутентификация
                                return RedirectToAction("Index", "Home");
                            }
                        }
					}
					
                }
                return Redirect("Loging");
            }
            else
            {
                return View("Registr");
            }

        }
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }


        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
               
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [Authorize]
        public IActionResult LogoutView()
        {
           
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}