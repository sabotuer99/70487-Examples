using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace OwinBasicAuth.BasicAuth
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly string _challenge;

        public BasicAuthenticationHandler(BasicAuthenticationOptions options)
        {
            _challenge = "Basic realm=" + options.Realm;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            var authzValue = Request.Headers.Get("Authorization");

            if(string.IsNullOrEmpty(authzValue) || !authzValue.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var token = authzValue.Substring("Basic ".Length).Trim();
            var claims = await TryGetPrincipalFromBasicCredentials(token, Options.Validator);

            if(claims == null)
            {
                return null;
            } else
            {
                var id = new ClaimsIdentity(claims, Options.AuthenticationType);
                return new AuthenticationTicket(id, new AuthenticationProperties());
            }
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if(Response.StatusCode == 401)
            {
                var challenge = Helper.LookupChallenge(
                    Options.AuthenticationType, 
                    Options.AuthenticationMode);
                if(challenge != null)
                {
                    Response.Headers.AppendValues("WWW-Authenticate", _challenge);
                }
            }

            return Task.FromResult<object>(null);
        }

        private async Task<IEnumerable<Claim>> TryGetPrincipalFromBasicCredentials(
            string token, Func<string, string, bool> validator)
        {
            string pair;
            try
            {
                pair = Encoding.UTF8.GetString(
                    Convert.FromBase64String(token));
            } catch (FormatException)
            {
                return null;
            } catch (ArgumentException)
            {
                return null;
            }

            var tokens = pair.Split(':');
            if(tokens.Length < 2)
            {
                return null;
            }

            var username = tokens[0];
            var password = tokens[1];

            if(validator(username, password))
            {
                var name = new Claim(ClaimTypes.Name, username);
                var auth = new Claim(ClaimTypes.AuthenticationMethod, "Basic");
                var claims = new List<Claim>();
                claims.Add(name);
                claims.Add(auth);
                return claims;
            }

            return null;
        }
    }
}
