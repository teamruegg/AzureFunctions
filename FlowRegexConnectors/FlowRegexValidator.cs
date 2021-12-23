using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Teamruegg.AzureFunctions
{
    public static class FlowRegexValidator
    {
        [FunctionName("FlowRegexValidator")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("FlowRegexValidator function triggered.");

            // Get value to be parsed by regex pattern
            string value = req.Query["value"];
            string pattern = req.Query["pattern"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            value = value ?? data?.value;

            return (System.Text.RegularExpressions.Regex.IsMatch(value, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)) ?
                    (ActionResult)new OkObjectResult(new { Result = true}) :
                    (ActionResult)new OkObjectResult(new { Result = false});

        }
    }
}
