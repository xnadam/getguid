using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GetGuid2
{
    public class GetGuid
    {
        private readonly ILogger _logger;

        public GetGuid(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetGuid>();
        }

        [Function("GetGuid")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("Started the GetGuid Function Call");

            string? numberOfGuidsText = req.Query["count"];
            int numberOfGuids = 1;
            List<string> guids = new();

            if (numberOfGuidsText != null && int.TryParse(numberOfGuidsText, out numberOfGuids))
            {
                _logger.LogInformation($"Number of Guids requested: {numberOfGuids}");
            }
            else
            {
                _logger.LogInformation($"Unknown number of Guids requested. Using 1.");
                numberOfGuids = 1;
            }

            for (int i = 0; i < numberOfGuids; i++)
            { 
                guids.Add( Guid.NewGuid().ToString() );
            }

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync( guids );
            

            return response;
        }
    }
}
