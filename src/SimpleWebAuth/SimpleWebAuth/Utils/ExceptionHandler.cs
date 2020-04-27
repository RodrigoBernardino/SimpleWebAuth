using System;

namespace SimpleWebAuth.Utils
{
    internal static class ExceptionHandler
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
            Exception newException;

            try
            {
                newException = (Exception)Activator.CreateInstance(type, message);
            }
            catch
            {
                newException = new Exception(message);
            }

            return newException;
        }

        public static string GenerateFriendlyMessage(Exception ex)
        {
            string realMessage = GetRealExceptionMessage(ex);

            return realMessage;
        }
    }
}
