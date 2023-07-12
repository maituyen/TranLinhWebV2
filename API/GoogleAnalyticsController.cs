using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;

namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAnalyticsController : ControllerBase
    {
        private async Task<UserCredential> GetCredential()
        {
            using (var stream = new FileStream("client_secret.json",
                 FileMode.Open, FileAccess.Read))
            {
                const string loginEmailAddress = "suadienthoaihaiphong157@gmail.com";
                return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { AnalyticsReportingService.Scope.Analytics },
                    loginEmailAddress, CancellationToken.None,
                    new FileDataStore("GoogleAnalyticsApiConsole"));
            }
        }

        [HttpGet("users")]
        public dynamic GetUsers()
        {
            try
            {
                var credential = GetCredential().Result;
                using (var svc = new AnalyticsReportingService(
                    new BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Google Analytics API Console"
                    }))
                {
                    var dateRange = new DateRange
                    {
                        StartDate = "2016-05-01",
                        EndDate = "2016-05-31"
                    };
                    var sessions = new Metric
                    {
                        Expression = "ga:sessions",
                        Alias = "Sessions"
                    };
                    var date = new Dimension { Name = "ga:date" };

                    var reportRequest = new ReportRequest
                    {
                        DateRanges = new List<DateRange> { dateRange },
                        Dimensions = new List<Dimension> { date },
                        Metrics = new List<Metric> { sessions },
                        ViewId = "<<your view id>>"
                    };
                    var getReportsRequest = new GetReportsRequest
                    {
                        ReportRequests = new List<ReportRequest> { reportRequest }
                    };
                    var batchRequest = svc.Reports.BatchGet(getReportsRequest);
                    var response = batchRequest.Execute();
                    List<dynamic> results = new List<dynamic>();
                    foreach (var x in response.Reports.First().Data.Rows)
                    {
                        results.Add(string.Join(", ", x.Dimensions) +
                        "   " + string.Join(", ", x.Metrics.First().Values));
                    }
                    return results;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
