using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFormAndWebApiLab
{
    public static class Constants
    {
        public const string AuthTokenName = "MyAuthToken";
        public const string AdminRoleName = "Administrator";
        public const string User1 = "user1";
        public const string EncryptedPassword = "����I��+�4p\u0019��\"���vi��\u007fa-�\u0016��"; // For "Welcome123@"
        public const string UnauthorizedJsonString = "{\"Message\":\"Authorization has been denied for this request.\"}";
        public const string UnauthorizedHttpsRequiredJsonString = "{\"Message\":\"Https is required for this request.\"}";
        public const string ApplicationJson = "application/json";
        public const string TextHtml = "text/html";
    }
}