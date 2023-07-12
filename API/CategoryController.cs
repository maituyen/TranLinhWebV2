using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Data;
using MyProject.Data.Entities;
using MyProject.Helpers;
using MyProject.ViewModels;
using MyProject.ViewModels.Category;
using SlugGenerator; 
using System.Security.Policy;
using System.Net.Http.Headers; 
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace MyProject.API
{
    [ApiController] 
    public class CategoryController : ApiBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly FormatString _formatString;
        public CategoryController(IConfiguration configuration)
        { 
            _configuration = configuration;
            _formatString = new FormatString();
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
        public void AsyncSave(dynamic data)
        {
            Models.Category category = new Models.Category();
            Models.Category parent = new Models.Category();
            if (data.parentId != null)
                parent = parent.GetByKiotViet((int)data.parentId);
            else
                parent.Id = 0;
            category = category.GetByKiotViet((int)data.categoryId);

            category.ParentId = parent.Id;
            category.Name = (string)data.categoryName;
            category.NameSEO = (string)data.categoryName;
            category.KiotVietId = (int)data.categoryId;
            category.SeoUrl = Helpers.SlugHelper.SlugNameUrl((string)data.categoryName);
            category.Type = "Product";
            category.CreatedAt = DateTime.Now;
            category.UpdatedAt = DateTime.Now;
            category.Save();
            if (((bool)data.hasChild))
            {
                foreach (var item in data.children)
                {
                    AsyncSave(item);
                }
            }
        }
        [HttpPost]
        [Route("async")]
        public async Task<dynamic> ASYNC()
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
                    var data= await responseMessage.Content.ReadAsAsync<dynamic>();
                    foreach (var item in data.data)
                    {
                        AsyncSave(item);
                    } 
                    return data;
                }
                else
                {
                    return BadRequest();
                }
            }
        }
        
        [HttpGet]
        [Route("get/{id}")]
        public dynamic Get(int id)
        {
            Models.Category category = new Models.Category(id);
            if (category.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Category>("Không tìm thấy danh mục"));
            }else
            {
                return Ok(category);
            }
        }
        
        [HttpGet]
        [Route("list")]
        public dynamic GetList(string type)
        {
            Models.Category category = new Models.Category();
            return Ok(category.GetByParent(0, type));
        }
        
        [HttpGet]
        [Route("create_url/{url}")]
        public dynamic CreateUrl(string url)
        {
            return Ok(new ApiResponseServer<string>(1, SlugHelper.SlugNameUrl(url)));
        }
        
        [HttpPost]
        [Route("save")]
        public dynamic Save(Models.Category data)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                return BadRequest(new ApiBadRequestResponse<Category>("Tên danh mục không được bỏ trống"));
            }
            if (string.IsNullOrEmpty(data.SeoUrl))
            {
                return BadRequest(new ApiBadRequestResponse<Category>("Đường dẫn không được bỏ trống"));
            }
            string url = SlugHelper.SlugNameUrl(data.SeoUrl);
            Models.Category category = new Models.Category(url);
            if (data.Id == -1)
            {
                if (SlugHelper.Exists(url))
                    return BadRequest(new ApiBadRequestResponse<Category>("Đường dẫn tồn tại trong hệ thống"));
            }
            else if (category.Id != -1 && category.Id != data.Id)
            {
                return BadRequest(new ApiBadRequestResponse<Category>("Đường dẫn tồn tại trong hệ thống"));
            }
            category = new Models.Category
            {
                Id = data.Id,
                Icon = data.Icon,
                Image = data.Image,
                BigImage = data.BigImage,
                Name = data.Name,
                NameSEO = data.NameSEO,
                ParentId = data.ParentId,
                IsShow = data.IsShow,
                IsDelete = data.IsDelete,
                SubDescription = data.SubDescription,
                Description = data.Description,
                Status = data.Status,
                SeoUrl = data.SeoUrl,
                Type = data.Type,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                KiotVietId = data.KiotVietId, 
            };
            string RootFoler = "";
            Models.Category root = new Models.Category();
            root=root.GetRootCategory(category);
            if (root.Id != category.Id)
                RootFoler = SlugHelper.SlugNameUrl(root.Name);

            if ((data.Icon ?? "").Contains("base64"))
                category.Icon = Helpers.Files.SaveFile(RootFoler, SlugHelper.SlugNameUrl(data.Name),  SlugHelper.SlugNameUrl(data.Name)+"_icon", data.Icon);

            if ((data.Image ?? "").Contains("base64"))
                category.Image = Helpers.Files.SaveFile(RootFoler, SlugHelper.SlugNameUrl(data.Name), SlugHelper.SlugNameUrl(data.Name) + "_small", data.Image);

            if ((data.Image ?? "").Contains("base64"))
                category.BigImage = Helpers.Files.SaveFile(RootFoler, SlugHelper.SlugNameUrl(data.Name), SlugHelper.SlugNameUrl(data.Name), data.Image);


            category = category.Save();
            if (category.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Models.Category>("Xảy ra lỗi, Danh mục chưa được lưu"));
            }
            else
            {
                return Ok(new ApiResponseServer<Models.Category>(1, "Danh mục được lưu thành công"));
            }
        }
        [HttpPost]
        [Route("delete")]
        public dynamic DeleteHastag(int id)
        {
            Models.Category category = new Models.Category(id);
            if (category.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Hastag>("Không tìm thấy danh mục"));
            }
            Helpers.ReturnClient returnClient = category.Delete();
            if (returnClient.sucess)
            {
                return Ok(new ApiResponseServer<Hastag>(1, "Xóa danh mục thành công"));
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse<Hastag>(returnClient.message));
            }
        }
        [HttpGet]
        [Route("test")]
        public void test()
        {
            Helpers.GoogleDrive googleDrive = new GoogleDrive();
            Helpers.GoogleDrive.CreateFolder("test"); 
        }
    }
}
