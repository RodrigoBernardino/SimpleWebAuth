using System;
using System.Configuration;

namespace SimpleWebAuth.Utils
{
    internal static class AuthConstants
    {
        public static string TokenValidAudience
        {
            get
            {
                string tokenValidAudience = ConfigurationManager.AppSettings["TokenValidAudience"];

                if (String.IsNullOrEmpty(tokenValidAudience))
                    throw new ApplicationException("It was not possible to find or convert the token valid audience at the Web.config file. Check the 'TokenValidAudience' appSettings key value.");

                return tokenValidAudience;
            }
        }

        public static string TokenValidIssuer
        {
            get
            {
                string tokenValidIssuer = ConfigurationManager.AppSettings["TokenValidIssuer"];

                if (String.IsNullOrEmpty(tokenValidIssuer))
                    throw new ApplicationException("It was not possible to find or convert the token valid issuer at the Web.config file. Check the 'TokenValidIssuer' appSettings key value.");

                return tokenValidIssuer;
            }
        }

        public static string LdapPath
        {
            get
            {
                string ldapPath = ConfigurationManager.AppSettings["LdapPath"];

                if (!String.IsNullOrEmpty(ldapPath))
                    throw new ApplicationException("It was not possible to find or convert the LDAP path at the Web.config file. Check the 'LdapPath' appSettings key value.");

                return ldapPath;
            }
        }

        public static string LdapDomain
        {
            get
            {
                string ldapDomain = ConfigurationManager.AppSettings["LdapDomain"];

                if (!String.IsNullOrEmpty(ldapDomain))
                    throw new ApplicationException("It was not possible to find or convert the LDAP domain at the Web.config file. Check the 'LdapDomain' appSettings key value.");

                return ldapDomain;
            }
        }
    }
}
