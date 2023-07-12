using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Constants;
using MyProject.Data;
using MyProject.Data.Entities;
using MyProject.ViewModels;
using Newtonsoft.Json;

namespace MyProject.API
{
    [ApiController]
    public class BannerController : ApiBaseController
    { 
        public BannerController()
        {
        }
        [HttpGet]
        [Route("list")]
        public dynamic List()
        {
            Models.Banner Banner = new Models.Banner();
            return Ok(Banner.GetAll());
           
        }
        [HttpGet]
        [Route("get")]
        public dynamic Get(int id)
        {
            Models.Banner Banner = new Models.Banner(id);
            if (Banner.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Banner>("Không tìm thấy Banner"));
            }
            else
            {
                return Ok(Banner);
            }
        }
        [HttpPost]
        [Route("save")]
        public dynamic Save(Models.Banner data)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                return BadRequest(new ApiBadRequestResponse<Banner>("Tên không hợp lệ"));
            }
            Models.Banner banner = new Models.Banner
            {
                Id = data.Id,
                LinkImage = data.LinkImage,
                LinkImageMobile = data.LinkImageMobile,
                Index = data.Index,
                Name = data.Name,
                BackgroundColor = data.BackgroundColor,
                Height = data.Height, 
            };
            if (data.LinkImage.Contains("base64"))
                banner.LinkImage = Helpers.Files.SaveFile("Banner", "", Helpers.SlugHelper.SlugNameUrl(data.Name), data.LinkImage);

            if (data.LinkImageMobile.Contains("base64"))
                banner.LinkImageMobile = Helpers.Files.SaveFile("Banner", "", Helpers.SlugHelper.SlugNameUrl(data.Name), data.LinkImageMobile);

            banner = banner.Save();
            if (banner.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Banner>("Xảy ra lỗi, Banner chưa được lưu"));
            }
            else
            {
                return Ok(new ApiResponseServer<Banner>(1, "Banner được lưu thành công"));
            }
        }
        [HttpPost]
        [Route("delete")]
        public dynamic DeleteBanner(int id)
        {
            Models.Banner Banner = new Models.Banner(id);
            if (Banner.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Banner>("Không tìm thấy Banner"));
            }
            Helpers.ReturnClient returnClient = Banner.Delete();
            if (returnClient.sucess)
            {
                return Ok(new ApiResponseServer<Banner>(1, "Xóa Banner thành công"));
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse<Banner>(returnClient.message));
            }
        }
    }
}
