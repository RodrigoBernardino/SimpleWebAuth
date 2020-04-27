using Microsoft.IdentityModel.Tokens;
using SimpleWebAuth.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleWebAuth.Token
{
    internal static class TokenGenerator
    {
        internal static async Task<string> CreateToken(TokenUser user)
        {
            DateTime issuedAt = DateTime.Now;
            DateTime expires = DateTime.UtcNow.Add(TimeSpan.FromDays(3650)); //ten years

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));

            foreach (KeyValuePair<string, string> claim in user.ClaimTypesValues)
                userClaims.Add(new Claim(claim.Key, claim.Value));

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userClaims);

            SigningCredentials signingCredentials = new SigningCredentials(WebAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(
                issuer: AuthConstants.TokenValidIssuer,
                audience: AuthConstants.TokenValidAudience,
                subject: claimsIdentity,
                notBefore: issuedAt,
                expires: expires,
                signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
