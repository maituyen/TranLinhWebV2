using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Constants;
using MyProject.Data;
using MyProject.Data.Entities;
using MyProject.Helpers;
using MyProject.ViewModels;
using MyProject.ViewModels.Hastag; 

namespace MyProject.API
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class HastagsController : ControllerBase
    {
        public HastagsController()
        { 
        }
        [HttpGet]
        [Route("categoriesrender")]
        public dynamic CategoriesRender()
        {
            Models.HastagRender hastagRender  = new Models.HastagRender();
            var result = hastagRender.GetAll(); 
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("getbytype")]
        public dynamic GetAll(int Type = -1)
        {
            Models.Hastag hastag = new Models.Hastag();
            var result = hastag.GetAll(Type);
            foreach (var item in result)
            {
                if (item.Type == 1)
                {
                    item.Name= HasTagHelper.GetDateNow(item.Code,item.Name);
                }
            }
            return Ok(result);
        }
        
        [HttpGet]
        [Route("get")]
        public dynamic Get(int id)
        {
            Models.Hastag hastag = new Models.Hastag(id);
            if (hastag.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Hastag>("Không tìm thấy Hastag"));
            }
            else
            {
                return Ok(hastag);
            }
        }
        [HttpPost]
        [Route("save")]
        public dynamic Save(Models.Hastag data)
        {
            if (data.Code == "#" || string.IsNullOrEmpty(data.Code))
            {
                return BadRequest(new ApiBadRequestResponse<Hastag>("Hastag không hợp lệ"));
            }
            if (!data.Code.Contains("#"))
            {
                return BadRequest(new ApiBadRequestResponse<Hastag>("Vui lòng nhập prefix # cho code"));
            } 
            Models.Hastag hastag = new Models.Hastag(data.Code);
            if(data.Id==-1)
            {
                if (hastag.Id != -1)
                    return BadRequest(new ApiBadRequestResponse<Hastag>("Hastag đã tồn tại"));
            }
            else
            {
                if(hastag.Id!=data.Id)
                    return BadRequest(new ApiBadRequestResponse<Hastag>("Hastag đã tồn tại"));
            }
            hastag = new Models.Hastag
            {
                Id = data.Id,
                Code = data.Code,
                Name = data.Name,
                Type = data.Type,
                KeywordId = data.KeywordId,
                Width = data.Width,
                Height = data.Height,
                Src = data.Src,
                AnchorLink = data.AnchorLink,
                Alt = data.Alt,
            };
            if (data.Src.Contains("base64"))
                hastag.Src = Helpers.Files.SaveFile("hastag", "", data.Code.Replace("#",""), data.Src);

            hastag = hastag.Save();
            if (hastag.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Hastag>("Xảy ra lỗi, Hastag chưa được lưu"));
            }
            else
            {
                return Ok(new ApiResponseServer<Hastag>(1, "Hastag được lưu thành công"));
            }
        } 
        [HttpPost] 
        [Route("delete")]
        public dynamic DeleteHastag(int id)
        {
            Models.Hastag hastag = new Models.Hastag(id);
            if (hastag.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Hastag>("Không tìm thấy Hastag"));
            }
            Helpers.ReturnClient returnClient = hastag.Delete();
            if (returnClient.sucess)
            {
                return Ok(new ApiResponseServer<Hastag>(1, "Xóa Hastag thành công"));
            }else
            {
                return BadRequest(new ApiBadRequestResponse<Hastag>(returnClient.message));
            }
        }  
    }
}
