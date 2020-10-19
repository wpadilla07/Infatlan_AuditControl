using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Infatlan_AuditControl
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e){

        }

        protected void Session_Start(object sender, EventArgs e){

        }

        protected void Application_BeginRequest(object sender, EventArgs e){
            HttpContext.Current.Response.AddHeader("X-Frame-Options", "SAMEORIGIN");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e){

        }

        protected void Application_Error(object sender, EventArgs e){

        }

        protected void Session_End(object sender, EventArgs e){

        }

        protected void Application_End(object sender, EventArgs e){

        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e){
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
        }
    }
}