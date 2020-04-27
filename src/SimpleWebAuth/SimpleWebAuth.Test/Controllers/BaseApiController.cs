using SimpleWebAuth.Test.Utils;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimpleWebAuth.Test.Controllers
{
    public class BaseApiController : ApiController
    {
        protected IHttpActionResult ExecuteFaultHandledOperation(Func<IHttpActionResult> operation)
        {
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                string friendlyMessage = ExceptionHandler.GenerateFriendlyMessage(ex);

                return InternalServerError(ExceptionHandler.GetExceptionWithFriendlyMessage(ex, friendlyMessage));
            }
        }

        protected async Task<IHttpActionResult> ExecuteFaultHandledOperation(Func<Task<IHttpActionResult>> operation)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                string friendlyMessage = ExceptionHandler.GenerateFriendlyMessage(ex);

                return InternalServerError(ExceptionHandler.GetExceptionWithFriendlyMessage(ex, friendlyMessage));
            }
        }
    }
}