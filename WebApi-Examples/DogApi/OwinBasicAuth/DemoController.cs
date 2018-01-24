using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
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
            var id = ClaimsPrincipal.Current;
            foreach(var claim in id.Claims)
            {
                Console.WriteLine(claim);
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Bingo!");
        }
    }
}
