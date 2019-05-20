using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebFormAndWebApiLab
{
    public partial class _default : System.Web.UI.Page
    {
        #region Page events
        SessionUser _sess = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Try to restore requestor's profile using the token supplied in the cookie
            if (HttpContext.Current.User.GetType() == typeof(SessionUser)) _sess = (SessionUser)HttpContext.Current.User;
        }
        protected void loginBtn_Click(object sender, EventArgs e)
        {
            // Authenticate the user here with supplied userid/password
            // We have only one user in our demo.
            if (string.Equals(userId.Text, Constants.User1, StringComparison.CurrentCultureIgnoreCase) && 
                EncryptString(pwd.Text)==Constants.EncryptedPassword)
            {
                // Create a session token and return it via cookie in every call.
                AddTokenIntoCookies(SessionManager.CreateSession(userId.Text.ToLower()));
                lbl.Text = "You have been logged in";
            }
            else
            {
                lbl.Text = "Invalid user id or password";
            }
        }
        protected void logoffBtn_Click(object sender, EventArgs e)
        {
            if (_sess == null)
                lbl.Text = "You are not logged in";
            else
            {
                Response.Cookies.Remove(Constants.AuthTokenName);
                SessionManager.KillSession((Guid)_sess.Token);
                lbl.Text = "You have been logged out";
            }
        }
        protected void becomeAdmin_Click(object sender, EventArgs e)
        {
            if (_sess != null)
            {
                if (!_sess.IsInRole(Constants.AdminRoleName))
                    _sess.AllowedRoles.Add(Constants.AdminRoleName);
            }
            ShowLoginStatus();
        }
        protected void becomeNormalUser_Click(object sender, EventArgs e)
        {
            // Remove admin role
            if (_sess != null) _sess.AllowedRoles.Remove(Constants.AdminRoleName);
            ShowLoginStatus();
        }
        #endregion
        #region Other code
        private void ShowLoginStatus()
        {
            if (_sess == null) lbl.Text = "Not logged in";
            else lbl.Text = string.Format("Logged in.  Admin={0}", _sess.IsInRole(Constants.AdminRoleName));
        }
        private void AddTokenIntoCookies(SessionUser sess)
        {
            var cookie = new System.Web.HttpCookie(Constants.AuthTokenName, sess.Token.ToString());
            cookie.Expires = DateTime.UtcNow.AddYears(1);
            cookie.HttpOnly = false;
            Response.Cookies.Add(cookie);
        }

        private Guid? ExtractSessionToken()
        {
            var cookie = Request.Cookies[Constants.AuthTokenName];
            if (cookie != null && (!string.IsNullOrWhiteSpace(cookie.Value))) return new Guid (cookie.Value);
            return null;
        }
        /// <summary>
        /// Password encryptor
        /// </summary>
        private string EncryptString(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] result;
            SHA256 shaM = new SHA256Managed();
            result = shaM.ComputeHash(data);
            return Encoding.UTF8.GetString(result, 0, result.Length);
        }
        #endregion 
    }
}