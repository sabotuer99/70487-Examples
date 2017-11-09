using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace DogApi.Controllers
{
    [RoutePrefix("api/Verbs")]
    public class VerbsController : ApiController
    {

        [HttpGet]
        // GET /api/Verbs
        [Route("")]
        public string Action()
        {
            return Request.Method.ToString();
        }

        [HttpGet]
        // GET /api/Verbs/async
        [Route("async")]
        public async Task<string> AsyncAction()
        {
            return await Task.FromResult(SomeSlowMethod() + "_async");
        }

        [HttpGet]
        // GET /api/sync
        [Route("~/api/sync")]
        public string SyncAction()
        {
            return SomeSlowMethod();
        }



        private string SomeSlowMethod()
        {
            Thread.Sleep(2000);

            return "Slooooow";
        }
    }
}