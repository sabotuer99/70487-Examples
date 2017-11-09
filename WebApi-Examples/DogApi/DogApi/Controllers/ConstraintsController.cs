using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DogApi.Controllers
{
    public class ConstraintsController : ApiController
    {
        [HttpGet]
        public string GetConstrainedValue(string alpha, int id = 0)
        {
            return alpha + " | " + id;
        }

        [HttpGet]
        [Route("api/Constraints/v2/{alpha:alpha}/{id:int?}")]
        public string GetConstrainedValueV2(string alpha, int id = 0)
        {
            return alpha + " | " + id + "... version 2.0";
        }

        [NonAction]
        public string IgnoreMe()
        {
            return "What the heck??";
        }

    }
}
