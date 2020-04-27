using System;

namespace SimpleWebAuth.Exceptions
{
    public class WebAuthException : Exception
    {
        public WebAuthException(string message)
            : base(message)
        { }
    }
}
