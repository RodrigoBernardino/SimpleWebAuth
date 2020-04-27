using Newtonsoft.Json.Linq;
using SimpleWebAuth.Token;
using SimpleWebAuth.Utils;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimpleWebAuth.Controller
{
    public class AuthController : ApiController
    {
        public AuthController()
        { }

        [AllowAnonymous]
        public async Task<IHttpActionResult> WindowsLogin()
        {
            try
            {
                TokenUser user = await UserValidator.ValidateWindowsUser();
                string token = await TokenGenerator.CreateToken(user);

                return Ok<string>(token);
            }
            catch (Exception ex)
            {
                string friendlyMessage = ExceptionHandler.GenerateFriendlyMessage(ex);
                return InternalServerError(ExceptionHandler.GetExceptionWithFriendlyMessage(ex, friendlyMessage));
            }
        }

        [AllowAnonymous]
        public async Task<IHttpActionResult> FormsLogin(JObject jsonData)
        {
            try
            {
                dynamic json = jsonData;
                string userName = json.username;
                string password = json.password;

                TokenUser user = await UserValidator.ValidateFormsUser(userName, password);
                string token = await TokenGenerator.CreateToken(user);

                return Ok<string>(token);
            }
            catch (Exception ex)
            {
                string friendlyMessage = ExceptionHandler.GenerateFriendlyMessage(ex);
                return InternalServerError(ExceptionHandler.GetExceptionWithFriendlyMessage(ex, friendlyMessage));
            }
        }
    }
}
