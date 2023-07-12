using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace MyProject.Helpers
{
    public class News
    {
        private string _doamin = "";

        private readonly IConfiguration _configuration;
        public News(IConfiguration configuration)
        {
            _configuration = configuration;
            _doamin = _configuration["Domain:Url"];

        }

        public async Task<dynamic> GetCategories()
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/news/categories";
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

        public async Task<dynamic> GetHot(int status)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/news/hot/"+ status;
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
        public async Task<dynamic> GetNewers()
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/newsers";
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
        public async Task<dynamic> GetNewest(int page, int size)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/newsest/"+page+"/"+size;
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
        public async Task<dynamic> GetByCategory(int id, int page, int size)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/news/category/"+id+"/" + page + "/" + size;
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

        public async Task<dynamic> GetNewsItem(int id)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/news/blog/" + id;
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
