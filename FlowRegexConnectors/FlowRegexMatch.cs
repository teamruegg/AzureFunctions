using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Teamruegg.AzureFunctions
{
    public static class FlowRegexMatch
    {
        [FunctionName("FlowRegexMatch")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("FlowRegexMatch function triggered.");

            // Get value to be parsed by regex pattern
            string value = req.Query["value"];
            string pattern = req.Query["pattern"];
            string responseMessage = "No match found";

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            value = value ?? data?.value;

            // Create new Regex
            Regex regex = new Regex(pattern);

            // Call Match on Regex instance
            Match match = regex.Match(value);
            
            // Test for Success
            if (match.Success)
            {
                responseMessage = match.Value;
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
