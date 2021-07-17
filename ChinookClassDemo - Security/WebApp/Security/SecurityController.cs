
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region additional namespaces
using Microsoft.AspNet.Identity.Owin;
#endregion

namespace WebApp.Security
{
    public class SecurityController
    {
        #region Employee/Customer IDs
        public int? GetCurrentUserEmployeeId(string userName)
        {
            int? id = null;
            var request = HttpContext.Current.Request;
            if (request.IsAuthenticated)
            {
                var manager = request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var appUser = manager.Users.SingleOrDefault(x => x.UserName == userName);
                if (appUser != null)
                    id = appUser.EmployeeId;
            }
            return id;
        }

        public int? GetCurrentUserCustomerId(string userName)
        {
            int? id = null;
            var request = HttpContext.Current.Request;
            if (request.IsAuthenticated)
            {
                var manager = request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var appUser = manager.Users.SingleOrDefault(x => x.UserName == userName);
                if (appUser != null)
                    id = appUser.CustomerId;
            }
            return id;
        }
        #endregion


        //Add this code to your SecurityController in your WebApp. This code will return an id value for either of Employee or Customer of the logged in user.You can then use the value to obtain the appropriate employee or customer record from your database.
            }
}