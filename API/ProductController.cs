using System.Data;
using System.Drawing.Printing;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Constants;
using MyProject.Data;
using MyProject.Data.Entities;
using MyProject.Extensions;
using MyProject.Helpers;
using MyProject.ViewModels;
using MyProject.ViewModels.Product;
using System.Linq;
namespace MyProject.API
{
    public class ProductController : ApiBaseController
    {
        private readonly IConfiguration _configuration; 

        public ProductController(
            IConfiguration configuration )
        {
            _configuration = configuration; 
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
        
        public async Task<dynamic> Async(int currentItem = 0)
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
                    string url = "/Products??orderBy=id&pageSize=100&currentItem=" + (currentItem * 100 + 1);
                    var responseMessage = await httpClient.GetAsync(url);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var data = await responseMessage.Content.ReadAsAsync<dynamic>();
                        
                        await Async(currentItem + 1);

                        return Ok(currentItem);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public async Task<dynamic> Info(string code)
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
                    var data= await responseMessage.Content.ReadAsAsync<dynamic>();
                    return new Models.Product();
                }
                else
                {
                    return BadRequest();
                }
            }
        }
        public async Task<dynamic> ProductByCategory(decimal id, int currentItem, int pageSize)
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
                        var data = await responseMessage.Content.ReadAsAsync<dynamic>();
                        Save1(data.data);
                        return Ok();
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
        [HttpPost]
        [Route("async")]
        public void AsyncSave()
        {
            try
            {
                Models.Category category = new Models.Category();
                List<Models.Category> categories = category.GetByParent(0, "-1");
                Models.Product product = new Models.Product();
                foreach (var item in categories)
                {
                    ProductByCategory(item.KiotVietId, 1, 100);
                    foreach (var sub in item.Categories)
                    {
                        ProductByCategory(sub.KiotVietId, 1, 100);
                    }
                } 
            }
            catch (Exception ex)
            {

            }
        }
        public async void Save1(dynamic data)
        {
            try
            {
                Models.Category category = new Models.Category();
                Models.Product product = new Models.Product();
                int VariantID = 0;
                int masterProductId = 0;
                foreach (var item in data)
                {
                    product = new Models.Product();
                    category = new Models.Category();
                    category = category.GetByKiotViet((int)item.categoryId);
                    if (((bool)item.hasVariants && (int)item.masterProductId != masterProductId))
                    {
                       Newtonsoft.Json.Linq.JObject  VariantResult = await ProductByMasterProductId((string)item.masterProductId);

                        Models.Product productVariant = new Models.Product
                        {
                            Name = (string)VariantResult["data"][0]["name"],
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            KiotVietId = (int)VariantResult["data"][0]["id"],
                            HasVariant = 1
                        };
                        masterProductId = (int)item.masterProductId;
                        productVariant = productVariant.Save();
                        VariantID = productVariant.Id;
                    }
                    product.VariantID = VariantID;
                    product.Name = (string)item.fullName;
                    product.CategoriesId = category.Id;
                    product.PriceSales = (decimal)item.basePrice;
                    product.SeoUrl = Helpers.SlugHelper.SlugNameUrl((string)item.name);
                    product.CreatedAt = DateTime.Now;
                    product.UpdatedAt = DateTime.Now;
                    product.Type = (string)item.type;
                    product.KiotVietId = (decimal)item.id;
                    product= product.Save();
                    // [21]	{ "attributes": [   { "productId": 1014292559,     "attributeName": "DUNG LƯỢNG",     "attributeValue": "256GB"   },   { "productId": 1014292559,     "attributeName": "MẦU SẮC",     "attributeValue": "Đỏ"   } ]}

                    if((bool)item.hasVariants)

                          {
                        foreach (var attr in item.attributes)
                        {
                            Models.ProductAttributes productAttributes = new Models.ProductAttributes
                            {
                                ProductId = product.Id,
                                AttributeName = attr.attributeName,
                                AttributeValue = attr.attributeValue,
                            };
                            productAttributes.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
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
                        var data = responseMessage.Content.ReadAsAsync<dynamic>().Result;
                        return data;
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
        [HttpGet]
        [Route("list")]
        public dynamic List(string Type)
        {
            Models.Product Product = new Models.Product();
            return Ok(Product.GetAll(Type, 0));
        }
        [HttpGet]
        [Route("get")]
        public dynamic Get(int id)
        {
            Models.Product Product = new Models.Product(id);
            if (Product.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Product>("Không tìm thấy thông tin"));
            }
            else
            {
                return Ok(Product);
            }
        }
        [HttpPost]
        [Route("save")]
        public dynamic Save( Models.Product data )
        {
           
            if (string.IsNullOrEmpty(data.SeoUrl))
            {
                return BadRequest(new ApiBadRequestResponse<Product>("Đường dẫn không hợp lệ"));
            }
            Models.Product product = new Models.Product(data.SeoUrl);
            if (data.Id == -1)
            {
                if (product.Id != -1)
                    return BadRequest(new ApiBadRequestResponse<Product>("Đường dẫn đã tồn tại"));
            }
            else
            {
                if (product.Id != data.Id)
                    return BadRequest(new ApiBadRequestResponse<Product>("Đường dẫn đã tồn tại"));
            }
            product = new Models.Product
            {
                Id = data.Id,
                VariantID = data.VariantID,
                Code = data.Code,
                CategoriesId = data.CategoriesId,
                Image = data.Image,
                BigImage = data.BigImage,
                Name = data.Name,
                Describe = data.Describe,
                SeoUrl = data.SeoUrl,
                Views = data.Views,
                IsPublish = data.IsPublish,
                MetaTitle = data.MetaTitle,
                MetaDesc = data.MetaDesc,
                IsNews = data.IsNews,
                IsAccessory = data.IsAccessory,
                EntryPrice = data.EntryPrice,
                PriceSales = data.PriceSales,
                PriceSalesOff = data.PriceSalesOff,
                SubsidyPrice = data.SubsidyPrice,
                Prepay = data.Prepay,
                AttributesColor = data.AttributesColor,
                AttributesSize = data.AttributesSize,
                AttributesType = data.AttributesType,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                HasVariant = (data.Attributes.Count > 0 ? 1 : 0)
            };
            if (data.Image.Contains("base64"))
                product.Image = Helpers.Files.SaveFile("Product", "", Helpers.SlugHelper.SlugNameUrl(data.Name), data.Image);
            if (data.BigImage.Contains("base64"))
                product.BigImage = Helpers.Files.SaveFile("Product", "", Helpers.SlugHelper.SlugNameUrl(data.Name), data.BigImage);

            product = product.Save();
            if (product.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Product>("Xảy ra lỗi, Product chưa được lưu"));
            }
            else
            {

                foreach (var item in data.Images)
                {
                    string Image = "", BigImage = "";
                    if (item.Image.Contains("base64"))
                        Image = Helpers.Files.SaveFile("Product", "", Helpers.SlugHelper.SlugNameUrl(data.Name), item.Image);
                    if (item.BigImage.Contains("base64"))
                        BigImage = Helpers.Files.SaveFile("Product", "", Helpers.SlugHelper.SlugNameUrl(data.Name), item.BigImage);
                    Models.ProductImages image = new Models.ProductImages()
                    {
                        Image = Image,
                        BigImage = BigImage,
                        ProductId = product.Id,
                        Caption = product.Name
                    };
                    image.Save();
                }
                foreach (var item in data.Hastags)
                {
                    Models.ProductHastag productHastag = new Models.ProductHastag()
                    {
                        ProductId = product.Id,
                        HastagsId = new Models.Hastag(item.Code).Id
                    };
                    productHastag.Save();
                }
                foreach (var item in data.Variants)
                {
                    item.Images = data.Images;
                    item.Hastags = data.Hastags;
                    item.Attributes = data.Attributes;
                    Save(item);
                }
                return Ok(new ApiResponseServer<Product>(1, "Product được lưu thành công"));
            }

        }
        [HttpPost]
        [Route("delete")]
        public dynamic DeleteProduct(int id)
        {
            Models.Product Product = new Models.Product(id);
            if (Product.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Product>("Không tìm thấy Product"));
            }
            Helpers.ReturnClient returnClient = new Helpers.ReturnClient();
            if (returnClient.sucess)
            {
                return Ok(new ApiResponseServer<Product>(1, "Xóa Product thành công"));
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse<Product>(returnClient.message));
            }
        }
    }
}
 