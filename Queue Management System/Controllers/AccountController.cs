using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Queue_Management_System.Models;
using System.Security.Claims;
using Npgsql;

namespace Queue_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private const string _tableName = "appusers";
        private IConfiguration _config;
        public AccountController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Login(string ReturnUrl = "/")
        {
            LoginVM loginViewModel = new LoginVM();
            loginViewModel.ReturnUrl = ReturnUrl;
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string userAuthenticationQuery = $"SELECT name, role, servicepointid " +
                                                $"FROM {_tableName} " +
                                                $"WHERE Name='{loginViewModel.Name}' " +
                                                $"AND Password='{loginViewModel.Password}'";
                AppUserVM appUser = AuthenticateAppUser(userAuthenticationQuery);
                if (appUser == null)
                {   
                    ViewBag.Message = "Name or Password is incorrect!";
                    return View(loginViewModel);
                }
                else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginViewModel.Name),
                        new Claim(ClaimTypes.Role, appUser.Role),
                        new Claim("ServicePointId", Convert.ToString(appUser.ServicePointId))
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                      new ClaimsPrincipal(claimsIdentity));

                    if (loginViewModel.ReturnUrl != "/")
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }
                    else if (appUser.Role == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (appUser.Role == "Service Provider")
                    {
                        return RedirectToAction("ServicePoint", "Queue");
                    }
                }
            }
            return View(loginViewModel);
        }
        public AppUserVM? AuthenticateAppUser(string userAuthenticationQuery)
        {
            AppUserVM appUser = null;

            string connectionString = _config.GetConnectionString("DefaultConnection");

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                // Prep command object.
                NpgsqlCommand command = new NpgsqlCommand(userAuthenticationQuery, connection);

                connection.Open();

                // Obtain a data reader via ExecuteReader()
                using (NpgsqlDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        appUser = new AppUserVM
                        {
                            Name = dataReader["name"].ToString(),
                            Role = dataReader["role"].ToString(),
                            ServicePointId = Convert.ToInt32(dataReader["servicepointid"])
                        };
                    }
                    dataReader.Close();                   
                }
                connection.Close();
            }
            if (appUser == null)            
                return null;
            return appUser;
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }
    }
}