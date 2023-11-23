using LerningAsp.Entyties;
using LerningAsp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Configuration;

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
		public  async Task<IActionResult> CheckRegistr(Registr reg)
		{
			
			if (ModelState.IsValid)
			{
				using (Context db = new Context())
				{
                    var persona = new User()
                    {
                        Login = reg.Login,
                        Email = reg.EMail,
                        Password = reg.Password,
                        Profile = new UserProfile()
                        {
                            ImagePath = "",
                            Description = "Default Description"
                            
                        }
                    };

                    db.Users.Add(persona);
                    db.SaveChanges();
                    await Authenticate(persona.Login);               
                    return View("Profile", persona);
                }                 
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
        public IActionResult FindFriends()
        {
            return View();
        }

		public IActionResult Loging()
		{
            return View();
        }
        [Authorize]
        public IActionResult GoToProfile()
        {
            using (Context db = new Context())
            {
                string userName = User.Identity.Name;
                User user = db.Users.FirstOrDefault(u => u.Login == userName);
                if (user != null)
                {
                  var userGoToProfile =  db.userProfile.FirstOrDefault(u => u.Id == user.Id);
                    user.Profile = userGoToProfile;
                    return View("Profile", user);
                                     
                }
            }
            return RedirectToAction("Error");
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
       
        public IActionResult Profile(User user)
        {
            
            return View(user);
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
        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile image)
        {

            if (image != null && image.Length > 0)
            {


                var uploadFolder = "C:\\Users\\user\\Desktop\\СайтикМатериалы\\";

                // Создать путь сохранения файла с оригинальным именем
                var filePath = Path.Combine(uploadFolder, image.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                using(Context db = new Context())
                {
                    string userName = User.Identity.Name;
                    User user = db.Users.FirstOrDefault(u => u.Login == userName);
                    if (user != null)
                    {
                        var userGoToProfile = db.userProfile.FirstOrDefault(u => u.Id == user.Id);
                        userGoToProfile = user.Profile;
                        userGoToProfile.ImagePath = filePath;
                        db.Update(userGoToProfile);
                        db.SaveChanges();

                        return View("Index");

                    }

                }
            }
            return RedirectToAction("Error");
        }

        public IActionResult GetImage(string imagePath)
        {
            using(Context db = new Context())
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(imageBytes, "image/jpeg");
            }

            
        }

        [HttpGet]
        public IActionResult Search(string SearchNameUser)
        {
            using(Context db = new Context())
            {

               List<User> userss = db.Users.Where(u => u.Login.Contains(SearchNameUser)).ToList();
                if (userss != null)
                {
                    foreach(var user in userss)
                    {
                        var userGoToProfile = db.userProfile.FirstOrDefault(u => u.Id == user.Id);
                         user.Profile.ImagePath = userGoToProfile.ImagePath;
                        try
                        {
                            byte[] imageBytes = System.IO.File.ReadAllBytes(user.Profile.ImagePath);
                            ViewBag.ImagePath = imageBytes;
                        }
                        catch
                        {

                        }
                           
                                        
                    }
                    
                    return View("FindFriends", userss);
                }
                else
                {
                    return Error();
                }
            }
        }



            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}