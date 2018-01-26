using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BasicAuthFilter
{
    class CheckAuthorizationAttribute : AuthorizationFilterAttribute
    {
        private IEnumerable<string> users;

        public CheckAuthorizationAttribute(params string[] users)
        {
            this.users = users;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string name = null;
            try
            {
                name = actionContext.ControllerContext.RequestContext.Principal.Identity.Name;
                if (!users.Contains(name, StringComparer.InvariantCultureIgnoreCase))
                {
                    throw new Exception("You are not on the list...");
                }
            } catch (Exception ex)
            {
                var message = new HttpResponseMessage(HttpStatusCode.Forbidden);
                message.Content = new StringContent("You do not have permission, sorry...\n" + 
                    ex.GetType().Name + ": " + ex.Message);
                throw new HttpResponseException(message);
            }
        }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            OnAuthorization(actionContext);
            return Task.FromResult(0);
        }
    }
}
