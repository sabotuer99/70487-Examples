using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinBasicAuth.BasicAuth
{
    //credit Dominick Baier, from his PluralSight Course
    public class BasicAuthenticationOptions : AuthenticationOptions
    {

        public string Realm;
        public Func<string, string, bool> Validator;

        public BasicAuthenticationOptions(string realm, 
            Func<string,string,bool> validator) : base("Basic")
        {
            Realm = realm;
            Validator = validator;
        }
    }
}
