using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebFormAndWebApiLab
{
    [MyAuthenticationFilter]
    public class DemoController : ApiController
    {
        // GET api/demo/Get/5 (Works for all users)
        [AllowAnonymous]
        public string Get(int id)
        {
            return "Single value";
        }
        
        // GET api/demo/Get (Works for any authenticated user)
        public IEnumerable<string> Get()
        {
            return new string[] { "List values", "value2" };
        }

        //Get api/demo/getTotalSales (Works only for admins)
        [Authorize(Roles = "Administrator")]
        public decimal GetTotalSales()
        {
            return 123000.98m;
        }
    }
}
