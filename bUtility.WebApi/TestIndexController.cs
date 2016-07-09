using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace bUtility.WebApi
{
	public class TestIndexController : ApiController
	{
		static string indexContent = @"
				<!DOCTYPE html>
				<html>
				<head>
					<title>test controller</title>
					<meta charset='utf - 8' />
				  </ head >
				  < body >
  
					  < h1 > hello test </ h1 >
				  </ body >
				  </ html >
			  ";

		HttpResponseMessage GetResponse(string content)
		{
			var response = new HttpResponseMessage
			{
				Content = new StringContent(content)
			};
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
			return response;
		}

		[HttpGet, HttpPost]
		public HttpResponseMessage Get()
		{
			if (Request.Method == HttpMethod.Post)
			{
				var rresponse = Request.CreateResponse(HttpStatusCode.Redirect);
				rresponse.Headers.Location = new Uri(Request.RequestUri.AbsoluteUri);
				return rresponse;
			}

			return GetResponse(indexContent);
		}

        [HttpGet, HttpPost]
        public string Ask4Text()
        {
            return "sample text";
        }

	}
}
