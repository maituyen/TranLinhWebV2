using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace MyProject.Helpers
{
    [Route("api/kiotviet")]
    [ApiController]
    public class KiotViet : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public KiotViet(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("products/currentItem/{currentItem}")]
        [HttpGet]
        public async Task<dynamic> Products(int currentItem)
        {
            try
            {
                var token = GetAccessToken().Result;

                var domainApi = _configuration["Domain:Kiotviet"];
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.BaseAddress = new Uri(domainApi);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    httpClient.DefaultRequestHeaders.Add("Retailer", _configuration["KiotVietToken:Shop"]);
                    string url = "/Products?orderBy=id&includeMaterial=true&pageSize=100&currentItem=" + currentItem;
                    var responseMessage = await httpClient.GetAsync(url);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return await responseMessage.Content.ReadAsAsync<dynamic>();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        [Route("products/category/{id}/currentItem/{currentItem}/size/{pageSize}")]
        [HttpGet]
        public async Task<dynamic> ProductByCategory(int id, int currentItem, int pageSize)
        {
            try
            {
                var token = GetAccessToken().Result;

                var domainApi = _configuration["Domain:Kiotviet"];
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.BaseAddress = new Uri(domainApi);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    httpClient.DefaultRequestHeaders.Add("Retailer", _configuration["KiotVietToken:Shop"]);
                    string url = "/Products?orderBy=createdDate&OrderDirection=DESC&currentItem=" + currentItem + "&pageSize=" + pageSize + "&categoryId=" + id;
                    var responseMessage = await httpClient.GetAsync(url);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return await responseMessage.Content.ReadAsAsync<dynamic>();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        [Route("products/name/{name}")]
        [HttpGet]
        public async Task<dynamic> ProductByRetailerId(string name)
        {
            try
            {
                var token = GetAccessToken().Result;

                var domainApi = _configuration["Domain:Kiotviet"];
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.BaseAddress = new Uri(domainApi);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    httpClient.DefaultRequestHeaders.Add("Retailer", _configuration["KiotVietToken:Shop"]);
                    string url = "/products?includeInventory=True&name=" + name;
                    var responseMessage = await httpClient.GetAsync(url);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return await responseMessage.Content.ReadAsAsync<dynamic>();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        [Route("products/masterProductId/{masterProductId}")]
        [HttpGet]
        public async Task<dynamic> ProductByMasterProductId(string masterProductId)
        {
            try
            {
                var token = GetAccessToken().Result;

                var domainApi = _configuration["Domain:Kiotviet"];
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.BaseAddress = new Uri(domainApi);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    httpClient.DefaultRequestHeaders.Add("Retailer", _configuration["KiotVietToken:Shop"]);
                    string url = "/products?orderBy=createdDate&OrderDirection=DESC&includeInventory=True&masterProductId=" + masterProductId;
                    var responseMessage = await httpClient.GetAsync(url);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return await responseMessage.Content.ReadAsAsync<dynamic>();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        [Route("products/code/{code}")]
        [HttpGet]
        public async Task<dynamic> ProductById(string code)
        {
            try
            {
                var token = GetAccessToken().Result;

                var domainApi = _configuration["Domain:Kiotviet"];
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.BaseAddress = new Uri(domainApi);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    httpClient.DefaultRequestHeaders.Add("Retailer", _configuration["KiotVietToken:Shop"]);
                    string url = "/products/code/" + code;
                    var responseMessage = await httpClient.GetAsync(url);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return await responseMessage.Content.ReadAsAsync<dynamic>();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [Route("categories")]
        [HttpGet]
        public async Task<dynamic> Categories()
        {
            try
            {
                var token = GetAccessToken().Result;

                var domainApi = _configuration["Domain:Kiotviet"];
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.BaseAddress = new Uri(domainApi);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    httpClient.DefaultRequestHeaders.Add("Retailer", _configuration["KiotVietToken:Shop"]);
                    string url = "/categories?hierachicalData=true";
                    var responseMessage = await httpClient.GetAsync(url);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return await responseMessage.Content.ReadAsAsync<dynamic>();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        [Route("categories/{id}")]
        [HttpGet]
        public async Task<dynamic> Catrgory(int id)
        {
            try
            {
                var token = GetAccessToken().Result;

                var domainApi = _configuration["Domain:Kiotviet"];
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.BaseAddress = new Uri(domainApi);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    httpClient.DefaultRequestHeaders.Add("Retailer", _configuration["KiotVietToken:Shop"]);
                    string url = "/categories/" + id;
                    var responseMessage = await httpClient.GetAsync(url);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return await responseMessage.Content.ReadAsAsync<dynamic>();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        private async Task<string> GetAccessToken()
        {
            var domainApi = _configuration["KiotVietToken:Domain"];

            using (var httpClient = new HttpClient())
            {
                var postData = new Dictionary<string, string>();
                postData.Add("client_id", _configuration["KiotVietToken:ClientId"]);
                postData.Add("client_secret", _configuration["KiotVietToken:ClientSecret"]);
                postData.Add("grant_type", "client_credentials");
                postData.Add("scopes", "PublicApi.Access");
                using (var content = new FormUrlEncodedContent(postData))
                {

                    httpClient.BaseAddress = new Uri(domainApi);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string url = "/connect/token";

                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsAsync<dynamic>().Result;
                        return data.access_token;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

    }
}
