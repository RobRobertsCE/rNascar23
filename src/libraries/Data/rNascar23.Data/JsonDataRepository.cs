using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace rNascar23.Data
{
    public class JsonDataRepository
    {
        private readonly ILogger<JsonDataRepository> _logger;

        protected JsonDataRepository(ILogger<JsonDataRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected string Get(string url)
        {
            var client = new RestClient(url);

            var request = new RestRequest(string.Empty, Method.Get);
            // Add HTTP headers
            request.AddHeader("User-Agent", "Nothing");

            // Execute the request and automatically deserialize the result.
            var result = client.Execute(request);

            var json = result.Content;

            if (json.Contains("<Error>"))
            {
                HandleXmlError(url, json);

                return string.Empty;
            }

            return json;
        }

        protected async Task<string> GetAsync(string url)
        {
            var client = new RestClient(url);

            var request = new RestRequest(string.Empty, Method.Get);
            // Add HTTP headers
            request.AddHeader("User-Agent", "Nothing");

            // Execute the request and automatically deserialize the result.
            var result = await client.ExecuteGetAsync(request);

            var json = result.Content;

            if (json.Contains("<Error>"))
            {
                HandleXmlError(url, json);

                return string.Empty;
            }

            return json;
        }

        protected virtual void HandleXmlError(string url, string xml)
        {
            try
            {
                var errorObject = (Error)new XmlSerializer(typeof(Error)).Deserialize(new StringReader(xml));

                _logger.LogInformation($"Error reading lap time data from {url}:\r\nCode: {errorObject.Code}\r\nMessage: {errorObject.Message}\r\nKey: {errorObject.Key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error handling XmlError. Xml: {xml}");
            }
        }
    }
}
