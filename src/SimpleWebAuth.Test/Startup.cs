using Microsoft.Owin;
using Owin;
using SimpleWebAuth.Test.App_Start;
using SimpleWebAuth.Token;
using Swashbuckle.Application;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

[assembly: OwinStartup(typeof(SimpleWebAuth.Test.Startup))]

namespace SimpleWebAuth.Test
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();

            httpConfiguration
                .EnableSwagger(api => api.SingleApiVersion("v1", "SimpleWebAuthTest"))
                .EnableSwaggerUi();

            //if you want to disable all token validation (no token needed in request header)
            //WebAuthConfig.DisableTokenValidation();

            WebAuthConfig.RegisterEndpoints(
                httpConfiguration,
                swaggerRequestsWithNoAuth: true,
                routesWithNoAuth: GetRoutesWithNoAuth());

            WebAuthConfig.RegisterUserValidation(UserValidationType.Custom, CustomUserValidation);

            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);
        }

        private List<string> GetRoutesWithNoAuth()
        {
            List<string> routesWithNoAuth = new List<string>();
            
            Assembly asm = Assembly.GetExecutingAssembly();
            List<MethodInfo> anonymousRoutes = asm.GetTypes()
                .Where(type => typeof(ApiController).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsDefined(typeof(AllowAnonymousAttribute)))
                .ToList();

            foreach (MethodInfo methodInfo in anonymousRoutes)
            {
                RouteAttribute routeAttribute = methodInfo.GetCustomAttribute<RouteAttribute>(true);
                if (routeAttribute != null)
                    routesWithNoAuth.Add(routeAttribute.Template);
            }

            return routesWithNoAuth;
        }

        public TokenUser CustomUserValidation(string userName = null, string password = null)
        {
            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
                throw new ApplicationException("Invalid user name or password");

            if (userName != "rodrigo.bernardino")
                throw new ApplicationException("Invalid user name");

            if(password != "Test@123")
                throw new ApplicationException("Invalid password");

            var userClaimsTypes = new ConcurrentDictionary<string, string>();
            userClaimsTypes.GetOrAdd("UserName", userName);
            userClaimsTypes.GetOrAdd("UserRole", "Admin");

            return new TokenUser
            {
                UserName = userName,
                ClaimTypesValues = userClaimsTypes
            };
        }
    }
}
