using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LoginSystem.Extensions;
using LoginSystem.Models.Interfaces;
using LoginSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LoginSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthentication _autentication;

        public LoginController(IAuthentication autentication)
        {
            _autentication = autentication;
        }

        public IActionResult Index()
        {
            var token = HttpContext?.Session?.GetString("_Token");
            var sessionExpired = SessionExpirationManager.Instance.IsSessionExpired(HttpContext);

            if (!string.IsNullOrEmpty(token) && !sessionExpired)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("Login");
        }

        [HttpPost]
        public IActionResult Signin(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var (success, user) = _autentication.SignIn(model.Login, model.Password);

                    if (!success)
                    {
                        TempData["ErrorMessage"] = $"Usuário ou senha incorretos";
                        return View("Login");
                    }

                    HttpContext.Session.SetString("_UserId", user.Id.ToString());
                    HttpContext.Session.SetString("_Login", user.Login);
                    HttpContext.Session.SetString("_Email", user.Email);
                    HttpContext.Session.SetString("_Name", user.Name);
                    HttpContext.Session.SetString("_SessionIsExpired", "false");

                    var claims = new[]{
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    };

                    byte[] randomKey = new byte[32];

                    using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                    {
                        rng.GetBytes(randomKey);
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(BitConverter.ToString(randomKey).Replace("-", string.Empty).ToLower()));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                    );

                    HttpContext.Session.SetString("_Token", new JwtSecurityTokenHandler().WriteToken(token));

                    return RedirectToAction("Index", "Home");
                }

                return View("Login");
            }
            catch (Exception error)
            {
                TempData["ErrorMessage"] = $"Ops ocorreu algum erro, {error.Message}";
                return RedirectToAction("Login");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("_UserId");
            HttpContext.Session.Remove("_Login");
            HttpContext.Session.Remove("_Token");
            HttpContext.Session.Remove("_Name");
            HttpContext.Session.Remove("_SessionIsExpired");
            HttpContext.Session.Remove("_Email");
            TempData["ErrorMessage"] = null;

            return View("Login");
        }
    }
}