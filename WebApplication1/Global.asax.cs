using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Routing;
using System.Web.Security;

namespace WebFormAndWebApiLab
{
    public class Global : System.Web.HttpApplication
    {
        #region Application life cycle events
        protected void Application_Start(object sender, EventArgs e)
        {
            SessionManager.LoadPastSessions();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
                    .Add(
                        new System.Net.Http.Formatting.RequestHeaderMapping(
                            "Accept",
                            Constants.TextHtml,
                            StringComparison.InvariantCultureIgnoreCase,
                            true,
                            Constants.ApplicationJson));
            GlobalConfiguration.Configure(ApiConfig.Register);
        }

        // Kill the request of not https
        internal protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!HttpContext.Current.Request.IsSecureConnection)
            {
                HttpContext.Current.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.Write(Constants.UnauthorizedHttpsRequiredJsonString);
                HttpContext.Current.Response.ContentType = Constants.ApplicationJson;
                (sender as HttpApplication).CompleteRequest();
            }
        }

        // This should be lightweiht as this method would get called for every request
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var token = GetCookieValue(HttpContext.Current, Constants.AuthTokenName);
            if (string.IsNullOrWhiteSpace(token)) return;
            HttpContext.Current.User = SessionManager.FindSession(new Guid(token));
        }
        protected void Application_End(object sender, EventArgs e)
        {
            SessionManager.SaveSessionsToFile();
        }
        #endregion

        #region Helper routines
        private string GetCookieValue(HttpContext actionContext, string cookieName)
        {
            var cookie = actionContext.Request.Cookies.Get(cookieName);
            if (cookie != null) return cookie.Value;
            return null;
        }
        #endregion
    }
}
