using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebFormAndWebApiLab
{
    public class SessionUser : IPrincipal
    {
        public Guid Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public List<string> AllowedRoles { get; set; }
        public Guid? GameId { get; set; }

        public SessionUser(string name)
        {
            AllowedRoles = new List<string>();
            Identity = new GenericIdentity(name);
        }
        public SessionUser()
        {
            AllowedRoles = new List<string>();
        }
        public bool IsInRole(string roleName)
        {
            return AllowedRoles.Contains(roleName);
        }
        public IIdentity Identity { get; set; }

    }
}