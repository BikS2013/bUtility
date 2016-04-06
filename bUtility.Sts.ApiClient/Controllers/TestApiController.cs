using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace bUtility.Sts.ApiClient.Controllers
{
    public class TestApiController : ApiController
    {
        [HttpGet, HttpPost]
        public string Sample()
        {
            return "sample text";
        }
    }
}
