using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using System.Data;


namespace AppBoxPro.Dapper
{
    public class Global : System.Web.HttpApplication
    {
        

        protected void Application_Start(object sender, EventArgs e)
        {
            
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }
        
        protected virtual void Application_EndRequest()
        {
            var context = HttpContext.Current.Items["__AppBoxProContext"] as IDbConnection;
            if (context != null)
            {
                context.Dispose();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}