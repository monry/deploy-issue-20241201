using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace DeployIssue20241201
{
    public class HttpExample
    {
        private readonly ILogger<HttpExample> _logger;

        public HttpExample(ILogger<HttpExample> logger)
        {
            _logger = logger;
        }

        [Function("HttpExample")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            var buildTimestampString = Environment.GetEnvironmentVariable("BUILD_TIMESTAMP");
            if (File.Exists("BUILD_TIMESTAMP"))
            {
                buildTimestampString = File.ReadAllText("BUILD_TIMESTAMP");
            }
            var buildDateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(long.TryParse(buildTimestampString, out var buildTimestamp) ? buildTimestamp : 0);
            var buildDateTimeOffsetJst = TimeZoneInfo.ConvertTime(buildDateTimeOffset, TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"));
            _logger.LogInformation("C# HTTP trigger function built at {BuildDateTime} processed a request", buildDateTimeOffsetJst.ToString());
            return new OkObjectResult($"Welcome to Azure Functions! Built at {buildDateTimeOffsetJst.ToString()}");
        }
    }
}
