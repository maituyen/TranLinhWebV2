using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MyProject.API
{

    [Route("api/[controller]")]
    [ApiController]
    public class CrawlingController : ControllerBase
    {
       
        [HttpGet]
        [Route("data/{slug}")]
        public async Task<dynamic> GetLastAsync(string slug )
        {
            var domain = "https://ecomws.didongviet.vn";
            var url = "/fe/v1/products/" + slug;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(domain);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseBody = await responseMessage.Content.ReadAsStringAsync();
                    dynamic output = JsonConvert.DeserializeObject(responseBody);

                    return output;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
