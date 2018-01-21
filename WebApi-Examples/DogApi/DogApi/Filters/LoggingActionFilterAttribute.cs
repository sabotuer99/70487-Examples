using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DogApi.Filters
{
    public class LoggingActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Debug.WriteLine("Before action...");
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Debug.WriteLine("After action...");
        }
    }
}