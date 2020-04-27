using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace SimpleWebAuth.Test.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            ConfigureJsonFormatter(config);
        }

        private static void ConfigureJsonFormatter(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}