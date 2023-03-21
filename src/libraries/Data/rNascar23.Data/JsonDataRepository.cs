using RestSharp;
using System.Threading.Tasks;

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

        protected async Task<string> GetAsync(string url)
        {
            var client = new RestClient(url);

            var request = new RestRequest(string.Empty, Method.Get);
            // Add HTTP headers
            request.AddHeader("User-Agent", "Nothing");

            // Execute the request and automatically deserialize the result.
            var result = await client.ExecuteGetAsync(request);

            return result.Content;
        }
    }
}
