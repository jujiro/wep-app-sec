using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebFormAndWebApiLab
{
    public class MyAuthenticationFilter : System.Web.Http.Filters.AuthorizationFilterAttribute
    {
        #region  Delegate(s)
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized) { Content = new StringContent(Constants.UnauthorizedJsonString, System.Text.Encoding.UTF8, Constants.ApplicationJson) };
                return;
            }
            if (NoAuthorizationNeeded(actionContext)) return;
            SessionUser sess = null;
            if (HttpContext.Current.User.GetType() == typeof(SessionUser)) sess = (SessionUser)HttpContext.Current.User;
            if (sess == null)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized) { Content = new StringContent(Constants.UnauthorizedJsonString, System.Text.Encoding.UTF8, Constants.ApplicationJson) };
                return;
            }
            base.OnAuthorization(actionContext);
        }
        #endregion
        /// <summary>
        /// If the action allows anonymous access then return true.
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static bool NoAuthorizationNeeded(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                       || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}
