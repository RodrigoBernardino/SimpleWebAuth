using System;

namespace SimpleWebAuth.Test.Utils
{
    public static class ExceptionHandler
    {
        public static string GetRealExceptionMessage(Exception ex)
        {
            while (true)
            {
                if (ex.InnerException == null) return ex.Message;
                ex = ex.InnerException;
            }
        }

        public static Exception GetExceptionWithFriendlyMessage(Exception ex, string message)
        {
            Type type = ex.GetType();
            Exception newException = (Exception)Activator.CreateInstance(type, message);
            return newException;
        }

        public static string GenerateFriendlyMessage(Exception ex)
        {
            string realMessage = GetRealExceptionMessage(ex);

            //this is how you change the message error to be more user friendly
            //if (realMessage.Contains(""))
            //    return "";

            return realMessage;
        }
    }
}