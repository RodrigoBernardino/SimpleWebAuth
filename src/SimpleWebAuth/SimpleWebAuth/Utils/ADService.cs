using hbehr.AdAuthentication;

namespace SimpleWebAuth.Utils
{
    internal class ADService
    {
        public AdAuthenticator AdAuthenticator { get; private set; }

        public ADService()
        {
            AdAuthenticator = new AdAuthenticator();
            AdAuthenticator.ConfigureSetLdapPath(AuthConstants.LdapPath)
                .ConfigureLdapDomain(AuthConstants.LdapDomain);
        }

        public AdUser VerifyWindowsAuthUser()
        {
            return AdAuthenticator.GetUserFromAd();
        }

        public AdUser VerifyAuthUser(string userName)
        {
            return AdAuthenticator.GetUserFromAdBy(userName);
        }

        public AdUser AuthenticateUser(string userName, string password)
        {
            return AdAuthenticator.SearchUserBy(userName, password);
        }
    }
}
