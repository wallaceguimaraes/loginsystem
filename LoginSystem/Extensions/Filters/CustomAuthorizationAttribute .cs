using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LoginSystem.Extensions.Filters
{
    public class CustomAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public CustomAuthorizationAttribute(ITempDataDictionaryFactory tempDataFactory)
        {
            _tempDataFactory = tempDataFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var sessionExpired = SessionExpirationManager.Instance.IsSessionExpired(context.HttpContext);
            var token = session.GetString("_Token");

            if (string.IsNullOrEmpty(token))
            {
                if (!context.ActionDescriptor.DisplayName.Contains("LoginController.Index") &&
                    !context.ActionDescriptor.DisplayName.Contains("LoginController.Signin"))
                    context.Result = new RedirectToActionResult("Index", "Login", null);
            }
            else
            {
                if (sessionExpired)
                {
                    ITempDataDictionary tempData = _tempDataFactory.GetTempData(context.HttpContext);
                    tempData["ErrorMessage"] = "Sessão expirada";

                    return;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                if (CheckTokenExpired(jwtToken))
                {
                    SessionExpirationManager.Instance.SetSessionExpired(context.HttpContext, true);
                    context.Result = new RedirectToActionResult("Index", "Login", null);
                }
            }

            base.OnActionExecuting(context);
        }

        private bool CheckTokenExpired(JwtSecurityToken jwtToken)
        {
            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                return true;
            }

            return false;
        }

        private bool CheckSessionExpired(ISession session)
        {
            var sessionTimeout = TimeSpan.FromMinutes(30);

            if (session.TryGetValue("LastActivity", out var lastActivityBytes) &&
                lastActivityBytes is byte[] lastActivityBytesArray)
            {
                var lastActivity = Encoding.UTF8.GetString(lastActivityBytesArray);
                if (DateTime.TryParse(lastActivity, out var lastActivityTime))
                {
                    if (DateTime.Now - lastActivityTime > sessionTimeout)
                        return true;
                }
            }

            session.Set("LastActivity", Encoding.UTF8.GetBytes(DateTime.Now.ToString()));

            return false;
        }
    }
}