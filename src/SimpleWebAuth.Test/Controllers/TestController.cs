using System.Web.Http;

namespace SimpleWebAuth.Test.Controllers
{
    public class TestController : BaseApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("api/test/get")]
        public IHttpActionResult Get()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                string test = "Test executed!";
                return Ok(test);
            });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/test/anonymousGet")]
        public IHttpActionResult AnonymousGet()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                string test = "Anonymous Test executed!";
                return Ok(test);
            });
        }

        [HttpPost]
        [Route("api/test/post")]
        public IHttpActionResult Post([FromBody]string text)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                string test = string.Format("Test executed! Received text: {0}", text);
                return Ok(test);
            });
        }
    }
}