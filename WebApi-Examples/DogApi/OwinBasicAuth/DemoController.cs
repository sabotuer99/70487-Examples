using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinBasicAuth
{
    public class DemoController : ApiController
    {
        [Authorize]
        [Route("api/Demo")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Bingo!");
        }
    }
}
