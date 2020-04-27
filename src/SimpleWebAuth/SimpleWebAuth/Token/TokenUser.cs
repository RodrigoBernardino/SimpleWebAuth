using System.Collections.Concurrent;

namespace SimpleWebAuth.Token
{
    public class TokenUser
    {
        public string UserName { get; set; }
        public ConcurrentDictionary<string, string> ClaimTypesValues { get; set; }
    }
}
