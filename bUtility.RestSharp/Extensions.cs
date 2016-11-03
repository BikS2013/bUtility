using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.RestSharp
{
    public static class Extensions
    {
        public static IRestResponse<R> ExecutePost<T, R>(this RestClient client, RestRequest request, T requestData, string address) where R : new()
        {
            var httpRequest = request ?? new RestRequest(address, Method.POST);
            httpRequest.AddHeader("Accept", "application/json");
            httpRequest.AddHeader("Content-type", "application/json");

            httpRequest.AddJsonBody(requestData);
            IRestResponse<R> resp = client.Execute<R>(httpRequest);
            return resp;
        }


    }
}
