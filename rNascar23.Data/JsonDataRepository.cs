using RestSharp;

namespace rNascar23.Data
{
    public class JsonDataRepository
    {
        protected string Get(string url)
        {
            var client = new RestClient(url);

            var request = new RestRequest(string.Empty, Method.Get);
            // Add HTTP headers
            request.AddHeader("User-Agent", "Nothing");

            // Execute the request and automatically deserialize the result.
            var result =  client.Execute(request);

            return result.Content;
        }
    }
}
