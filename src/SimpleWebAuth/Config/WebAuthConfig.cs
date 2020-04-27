using Microsoft.IdentityModel.Tokens;
using SimpleWebAuth.Handler;
using SimpleWebAuth.Token;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;

namespace SimpleWebAuth
{
    public static class WebAuthConfig
    {
        public delegate TokenUser CustomUserValidation(string UserName = null, string Password = null);

        internal static IEnumerable<string> RoutesWithNoAuth = new List<string>();
        internal static bool SwaggerRequestsWithNoAuth { get; private set; }
        internal static UserValidationType ValidationType { get; private set; }
        internal static SymmetricSecurityKey SecurityKey { get; private set; }
        internal static CustomUserValidation CustomUserValidationMethod { get; private set; }

        internal static bool AuthDisabled = false;

        public static void RegisterEndpoints(HttpConfiguration config, bool swaggerRequestsWithNoAuth = false, IEnumerable<string> routesWithNoAuth = null)
        {
            SwaggerRequestsWithNoAuth = swaggerRequestsWithNoAuth;

            if (routesWithNoAuth != null)
                RoutesWithNoAuth = routesWithNoAuth;

            GenerateSecurityKey();

            config.MessageHandlers.Add(new TokenValidationHandler());

            config.Routes.MapHttpRoute(
                name: "WindowsAuthApi",
                routeTemplate: "auth/windowsLogin/{id}",
                defaults: new
                {
                    controller = "Auth",
                    action = "WindowsLogin",
                    id = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "FormsAuthApi",
                routeTemplate: "auth/formsLogin/{id}",
                defaults: new
                {
                    controller = "Auth",
                    action = "FormsLogin",
                    id = RouteParameter.Optional
                }
            );
        }

        public static void RegisterUserValidation(UserValidationType validationType, CustomUserValidation customUserValidationMethod = null)
        {
            ValidationType = validationType;
            CustomUserValidationMethod = customUserValidationMethod;
        }

        public static void DisableTokenValidation()
        {
            AuthDisabled = true;
        }

        private static void GenerateSecurityKey()
        {
            //generate new secretKey
            //byte[] key = new byte[32];
            //RandomNumberGenerator.Create().GetBytes(key);
            //string secretKey = TextEncodings.Base64Url.Encode(key);

            string secretKey = "aBtrddQir6IkYUULdAuhvcPYtYWbYfjbDSXUyTg7EiM";
            SecurityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
        }
    }
}
