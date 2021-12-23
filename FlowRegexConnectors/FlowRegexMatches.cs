using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Teamruegg.AzureFunctions
{
    public static class FlowRegexMatches
    {
        [FunctionName("FlowRegexMatches")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("FlowRegexMatches function triggered.");

            // Get value to be parsed by regex pattern
            string value = req.Query["value"];
            string pattern = req.Query["pattern"];
            //string responseMessage = "No match found";

            // Create a list of matchResults
            var matchResults = new List<MatchResults>();


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            value = value ?? data?.value;

            // Get a collection of matches
            MatchCollection matches = Regex.Matches(value, pattern);
        
            // Use foreach-loop
            foreach (Match match in matches)
            {
                foreach (Capture capture in match.Captures)
                {
                    matchResults.Add(new MatchResults{ Index = capture.Index, Value = capture.Value} );
                }
            }

            string responseMessage = JsonConvert.SerializeObject(matchResults);

            return new OkObjectResult(responseMessage);
        }

        // Match result class for typed collection  
        private class MatchResults
        {
            public int Index { get; set; }
            public string Value { get; set; }
        }
    }
}
