using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AbsensiTTL
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        protected void Application_AuthenticateRequest(Object objectSender, EventArgs e)
        {
            if (this.User != null)
            {
                List<string> administrator  = new List<string>();
                administrator.Add("287030342");
                administrator.Add("286030234");
                administrator.Add("291030227");


                if(administrator.Contains(this.User.Identity.Name))
                {
                    List<string> roles = new List<string>();
                    roles.Add("Administrator");

                    Context.User = new GenericPrincipal(this.User.Identity, roles.ToArray());
                }                
            }
            
        }

    }
}
