using Dao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebWriterV2.DI
{
    public class PrincipalUser : IPrincipal
    {
        public User User { get; }

        public PrincipalUser(User user)
        {
            Identity = new IdentityUser(user);
            User = user;
        }

        public IIdentity Identity { get; }

        public bool IsInRole(string role)
        {
            var CurrentUser = Identity as IdentityUser;
            return role == CurrentUser.User?.UserType.ToString();
        }
    }

    public class IdentityUser : IIdentity
    {
        public IdentityUser(User user)
        {
            User = user;
        }

        public User User { get; }

        public string Name => User?.Name;

        public string AuthenticationType => User?.UserType.ToString();

        public bool IsAuthenticated => User != null;
    }
}