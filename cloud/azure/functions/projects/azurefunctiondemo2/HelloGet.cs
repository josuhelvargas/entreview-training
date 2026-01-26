using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace azurefunctiondemo2
{
    public static class HelloGet
    {
        [FunctionName("HelloGet")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hello")] HttpRequest req,
            ILogger log)
        {
            var name = req.Query["name"].ToString();

            if (string.IsNullOrWhiteSpace(name))
            {
                return new BadRequestObjectResult(new
                {
                    error = "Missing query param 'name'. Example: /api/hello?name=Josue"
                });
            }

            return new OkObjectResult(new
            {
                message = "Hello Azure",
                name,
                utc = System.DateTime.UtcNow
            });
        }
    }
}
