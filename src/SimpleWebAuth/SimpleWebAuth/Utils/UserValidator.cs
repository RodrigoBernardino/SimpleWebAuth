using hbehr.AdAuthentication;
using SimpleWebAuth.Exceptions;
using SimpleWebAuth.Token;
using System.Threading.Tasks;

namespace SimpleWebAuth.Utils
{
    internal static class UserValidator
    {
        public static async Task<TokenUser> ValidateWindowsUser()
        {
            switch (WebAuthConfig.ValidationType)
            {
                case UserValidationType.AD:
                    return await ValidateWindowsUserInAD();
                case UserValidationType.Custom:
                    return await ValidateWindowsUserInCustom();
                default:
                    throw new WebAuthException("Invalid UserValidationType.");
            }
        }

        public static async Task<TokenUser> ValidateFormsUser(string userName, string password)
        {
            switch (WebAuthConfig.ValidationType)
            {
                case UserValidationType.AD:
                    return await ValidateFormsUserInAD(userName, password);
                case UserValidationType.Custom:
                    return await ValidateFormsUserInCustom(userName, password);
                default:
                    throw new WebAuthException("Invalid UserValidationType.");
            }
        }

        private static async Task<TokenUser> ValidateWindowsUserInAD()
        {
            ADService adService = new ADService();
            AdUser windowsUser = adService.VerifyWindowsAuthUser();
            if (windowsUser == null)
                throw new WebAuthException("Windows user not found in AD.");

            return new TokenUser { UserName = windowsUser.Name };
        }

        private static async Task<TokenUser> ValidateWindowsUserInCustom()
        {
            if (WebAuthConfig.CustomUserValidationMethod == null)
                throw new WebAuthException("Custom user validation method not found.");

            return (TokenUser)WebAuthConfig.CustomUserValidationMethod.DynamicInvoke();
        }

        private static async Task<TokenUser> ValidateFormsUserInAD(string userName, string password)
        {
            ADService adService = new ADService();
            AdUser formsUser = adService.AuthenticateUser(userName, password);
            if (formsUser == null)
                throw new WebAuthException("Windows user not found in AD.");

            return new TokenUser { UserName = formsUser.Name };
        }

        private static async Task<TokenUser> ValidateFormsUserInCustom(string userName, string password)
        {
            if (WebAuthConfig.CustomUserValidationMethod == null)
                throw new WebAuthException("Custom user validation method not found.");

            return (TokenUser)WebAuthConfig.CustomUserValidationMethod.DynamicInvoke(userName, password);
        }
    }
}
