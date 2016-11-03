using RestSharp;

namespace bUtility.RestSharp
{
    public static class Extensions
    {
        public static IRestResponse<R> ExecutePost<T, R>(this RestClient client, RestRequest request, T requestData, string address=null) where R : new()
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
