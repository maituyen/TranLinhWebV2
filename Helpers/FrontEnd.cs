using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using MyProject.ViewModels;
using MyProject.ViewModels.Product;
using Newtonsoft.Json;

namespace MyProject.Helpers
{
    public class FrontEnd
    {
        private string _doamin = "";

        private readonly IConfiguration _configuration;
        public FrontEnd(IConfiguration configuration)
        {
            _configuration = configuration;
            _doamin = _configuration["Domain:Url"];
        }
        public bool IsMobile(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return false;
            }

            // Mobile
            const string mobileRegex =
                "blackberry|iphone|mobile|windows ce|opera mini|htc|sony|palm|symbianos|ipad|ipod|blackberry|bada|kindle|symbian|sonyericsson|android|samsung|nokia|wap|motor";

            if (Regex.IsMatch(userAgent, mobileRegex, RegexOptions.IgnoreCase)) 
                return true;
            // Not mobile 
            return false;
        }

        public async Task<dynamic> TopHome()
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/home/top";
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

        public async Task<List<FrontEndFooterVm>> Footer()
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/home/footer";
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseBody = await responseMessage.Content.ReadAsStringAsync();
                    var output = JsonConvert.DeserializeObject<List<FrontEndFooterVm>>(responseBody);
                    return output;
                }
                return null;
            }
        }

        public async Task<dynamic> ListProductHot()
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/home/hot";
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsAsync<List<WebConfigVm>>();
                    foreach (var res in responseBody)
                    {
                        foreach (var item in res.WebConfigProducts)
                        {
                            item.Product.PriceSale = item.Product.Sale * item.Product.KiotVietPrice / 100;
                            item.Product.FormatPrice = item.Product.KiotVietPrice;
                            item.Product.PriceSaleLast = item.Product.KiotVietPrice - item.Product.Sale * item.Product.KiotVietPrice / 100;
                            item.Product.FormatPrepay = item.Product.Prepay * item.Product.KiotVietPrice / 100;
                            item.Product.FormatName = string.Format(slugName((string)item.Product.Name));

                        }
                    }
                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<dynamic> ListProductBody()
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/home/body";
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsAsync<List<WebConfigVm>>();
                    foreach (var res in responseBody)
                    {
                        foreach (var item in res.WebConfigProducts)
                        {
                            var product = item.Product ?? new ProductConfigVm();
                            product.PriceSale = product.Sale * product.KiotVietPrice / 100;
                            product.FormatPrice = product.KiotVietPrice;
                            product.PriceSaleLast = product.KiotVietPrice - product.Sale * product.KiotVietPrice / 100;
                            product.FormatPrepay = product.Prepay * product.KiotVietPrice / 100;
                            product.FormatName = string.Format(slugName(product.Name));
                        }
                    }
                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<FrontEndFooterVm>> ListKeyHot()
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/keyword/hot";
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseBody = await responseMessage.Content.ReadAsStringAsync();
                    var output = JsonConvert.DeserializeObject<List<FrontEndFooterVm>>(responseBody);
                    return output;
                }
                else
                {
                    return null;
                }
            }
        }

        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        string slugName(string str)
        {
            return SlugHelper.SlugNameUrl(str);
        }

        string slugNameSearch(string str)
        {
            str = str.ToLower();
            return SlugGenerator.SlugGenerator.GenerateSlug(str);
        }

        public async Task<dynamic> ListMenu()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/menu";
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsAsync<dynamic>();

                    foreach (var item in responseBody)
                    {
                        item.FormatName = string.Format(slugName((string)item.Name));
                        item.Url = $"{item.FormatName}.html";
                    }
                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<dynamic> ListMenuOld(int status)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/menu/" + status;
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsAsync<dynamic>();

                    /* foreach (var item in responseBody)
                    {
                        item.FormatName = string.Format(slugName((string)item.Name));
                        item.Url = item.FormatName + ".html";
                    }*/
                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }


        public async Task<dynamic> CategoryById(int id)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/category/" + id;
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsAsync<dynamic>();

                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<dynamic> GetCategoryBySlug(string slug)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/get-category-by-slug/" + slug;
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsAsync<dynamic>();

                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<dynamic> CategoryBySlugId(int id)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/category/slug/" + id;
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsAsync<dynamic>();

                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<dynamic> GetInstallmentByCategoryId(int page, int size)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/Installment/category/page/" + page + "/size/" + size;
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsAsync<dynamic>();
                    foreach (var item in responseBody.Data)
                    {
                        item.PriceSale = item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrice = item.KiotVietPrice;
                        item.PriceSaleLast = item.KiotVietPrice - item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrepay = item.Prepay * item.KiotVietPrice / 100;
                        item.FormatName = string.Format(slugName((string)item.Name));
                    }
                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<dynamic> GetProductByCategoryId(int categoryId, int page, int size)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/product/category/" + categoryId + "/page/" + page + "/size/" + size;
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsAsync<dynamic>();
                    foreach (var item in responseBody.Data)
                    {
                        item.PriceSale = item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrice = item.KiotVietPrice;
                        item.PriceSaleLast = item.KiotVietPrice - item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrepay = item.Prepay * item.KiotVietPrice / 100;
                        item.FormatName = string.Format(slugName((string)item.Name));
                    }
                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<dynamic> GetProductByCategoryIdPrice(List<int> categoryIds, int page, int size, int start, int end)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/product/category/page/" + page + "/size/" + size + "/price/" + start + "/" + end;
                var content = new
                {
                    categoryIds = categoryIds,
                };
                var responseMessage = await httpClient.PostAsJsonAsync(url, content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsAsync<dynamic>();
                    foreach (var item in responseBody.Data)
                    {
                        item.PriceSale = item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrice = item.KiotVietPrice;
                        item.PriceSaleLast = item.KiotVietPrice - item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrepay = item.Prepay * item.KiotVietPrice / 100;
                        item.FormatName = string.Format(slugName((string)item.Name));
                    }
                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<dynamic> GetProductByCategoryIdKeyWordPrice(int page, int size, int start, int end, int keyword)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/product/page/" + page + "/size/" + size + "/price/" + start + "/" + end + "/keyword/" + keyword;
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsAsync<dynamic>();
                    
                    foreach (var item in responseBody.Data)
                    {
                        item.PriceSale = item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrice = item.KiotVietPrice;
                        item.PriceSaleLast = item.KiotVietPrice - item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrepay = item.Prepay * item.KiotVietPrice / 100;
                        item.FormatName = string.Format(slugName((string)item.Name));
                    }
                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<dynamic> GetProductBySlug(string slug)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/product/" + slug;
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var item = await responseMessage.Content.ReadAsAsync<dynamic>();
                    item.PriceSale = item.Sale * item.KiotVietPrice / 100;
                    item.FormatPrice = item.KiotVietPrice;
                    item.PriceSaleLast = item.KiotVietPrice - item.Sale * item.KiotVietPrice / 100;
                    item.FormatPrepay = item.Prepay * item.KiotVietPrice / 100;
                    item.FormatName = string.Format(slugName((string)item.Name));

                    return new
                    {
                        Detail = item
                    };
                }
                else
                {
                    return null;
                }
            }
        }


        public async Task<bool> CheckProductExistBySlug(string slug)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = $"/api/frontend/product/{slug}/check-exist";
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var item = await responseMessage.Content.ReadAsAsync<bool>();
                    return item;
                }

                return false;
            }
        }

        public async Task<dynamic> GetProductSuggest(string masterProductId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/product/suggest/" + masterProductId;
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {

                    var items = await responseMessage.Content.ReadAsAsync<dynamic>();
                    return items;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<dynamic> GetProductTagById(string Tag)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/product/tags/" + Tag;
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var items = await responseMessage.Content.ReadAsAsync<dynamic>();
                    foreach (var item in items)
                    {
                        item.PriceSale = (int)item.Sale * (int)item.KiotVietPrice / 100;
                        item.FormatPrice = (int)item.KiotVietPrice;
                        item.PriceSaleLast = (int)item.KiotVietPrice - (int)item.Sale * (int)item.KiotVietPrice / 100;
                        item.FormatPrepay = item.Prepay * item.KiotVietPrice / 100;
                        item.FormatName = string.Format(slugName((string)item.Name));

                    }

                    return items;
                }
                else
                {
                    return null;
                }
            }
        }
        public class ObjProduct
        {
            public string KeySearch { get; set; }
            public string Name { get; set; }
            public string FormatName { get; set; }
            public string Capacity { get; set; }
            public int Id { get; set; }
            public int TagProductId { get; set; }
            public int KiotVietPrice { get; set; }
        }
        public async Task<dynamic> GetProductOrderById(int Id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/product/order/" + Id;
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var item = await responseMessage.Content.ReadAsAsync<dynamic>();
                    item.PriceSale = item.Sale * item.KiotVietPrice / 100;
                    item.FormatPrice = item.KiotVietPrice;
                    item.PriceSaleLast = item.KiotVietPrice - item.Sale * item.KiotVietPrice / 100;
                    item.PriceSaleLastNumber = item.KiotVietPrice - item.Sale * item.KiotVietPrice / 100;
                    item.FormatPrepay = item.Prepay * item.KiotVietPrice / 100;
                    item.FormatName = string.Format(slugName((string)item.Name));
                    return item;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<dynamic> GetProductNewBySearching(string value)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/product/search/IsNew/" + slugNameSearch(value);
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var items = await responseMessage.Content.ReadAsAsync<dynamic>();
                    foreach (var item in items.Data)
                    {
                        item.PriceSale = item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrice = item.KiotVietPrice;
                        item.PriceSaleLast = item.KiotVietPrice - item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrepay = item.Prepay * item.KiotVietPrice / 100;
                        item.FormatName = string.Format(slugName((string)item.Name));
                    }
                    return items;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<dynamic> GetProductOldBySearching(string value)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/product/search/IsOld/" + slugNameSearch(value);
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var items = await responseMessage.Content.ReadAsAsync<dynamic>();
                    foreach (var item in items.Data)
                    {
                        item.PriceSale = item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrice = item.KiotVietPrice;
                        item.PriceSaleLast = item.KiotVietPrice - item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrepay = item.Prepay * item.KiotVietPrice / 100;
                        item.FormatName = string.Format(slugName((string)item.Name));
                    }
                    return items;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<dynamic> GetProductAccessoryBySearching(string value)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_doamin);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "/api/frontend/product/search/Accessory/" + slugNameSearch(value);
                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var items = await responseMessage.Content.ReadAsAsync<dynamic>();
                    foreach (var item in items.Data)
                    {
                        item.PriceSale = item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrice = item.KiotVietPrice;
                        item.PriceSaleLast = item.KiotVietPrice - item.Sale * item.KiotVietPrice / 100;
                        item.FormatPrepay = item.Prepay * item.KiotVietPrice / 100;
                        item.FormatName = string.Format(slugName((string)item.Name));
                    }
                    return items;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
