using Microsoft.IdentityModel.Tokens;
using SimpleWebAuth.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SimpleWebAuth.Handler
{
    internal class TokenValidationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;

            if (IgnoreRouteAuth(request) || WebAuthConfig.AuthDisabled)
            {
                return base.SendAsync(request, cancellationToken);
            }

            //determine whether a jwt exists or not
            if (!TryRetrieveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
            }

            try
            {
                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = AuthConstants.TokenValidAudience,
                    ValidIssuer = AuthConstants.TokenValidIssuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = WebAuthConfig.SecurityKey
                };

                //extract and assign the user of the jwt
                Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out securityToken);

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException e)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
                if (DateTime.UtcNow < expires)
                    return true;

            return false;
        }

        private bool IgnoreRouteAuth(HttpRequestMessage request)
        {
            bool isIgnoredSwaggerRoute = false;
            if (WebAuthConfig.SwaggerRequestsWithNoAuth && request.Headers.Referrer != null)
                isIgnoredSwaggerRoute = request.Headers.Referrer.AbsoluteUri.Contains("swagger");

            return isIgnoredSwaggerRoute
                || request.Method == HttpMethod.Options
                || request.RequestUri.LocalPath == "/"
                || request.RequestUri.AbsoluteUri.Contains("formsAuth")
                || request.RequestUri.AbsoluteUri.Contains("windowsAuth")
                || WebAuthConfig.RoutesWithNoAuth.Any(r => request.RequestUri.AbsoluteUri.Contains(r));
        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
                return false;

            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

            return true;
        }
    }
}
