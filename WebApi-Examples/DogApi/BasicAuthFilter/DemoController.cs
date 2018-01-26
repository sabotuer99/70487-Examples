using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace BasicAuthFilter
{
    public class DemoController : ApiController
    {
        [Authorize]
        [Route("api/Demo")]
        [CheckAuthorization("troy", "bob", "service account")]
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
