using System;
using MyProject.Data.Entities;
using MyProject.ViewModels;

namespace MyProject.Models
{
	public class WebConfigImage
    {
        Helpers.Database db = new Helpers.Database();
        public int WebconfigId { set; get; } = -1;
        public int BannerId { set; get; }
        public int Index { set; get; }
        public string LinkProduct { set; get; }
        public string ImageId { set; get; }
        public Banner  BannerInfo { set; get; }
        public WebConfigImage()
        { 
        }
        public WebConfigImage(int Id)
        {
            LoadData(GetById(Id));
        } 
        public void LoadData(WebConfigImage data)
        {
            this.WebconfigId = data.WebconfigId;
            this.BannerId = data.BannerId;
            this.Index = data.Index;
            this.LinkProduct = data.LinkProduct;
            this.ImageId = data.ImageId;
            this.BannerInfo = new Banner(this.BannerId);
        }
       
        public WebConfigImage GetById(int Id)
        {
            try
            {
                return db.Query<WebConfigImage>("WebConfigImage_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new WebConfigImage();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfigImage();
            }
        }
        public List<WebConfigImage> GetByWebConfigId(int WebconfigId)
        {
            try
            {
                var data= db.Query<WebConfigImage>("WebConfigImage_SelectByWebConfigId", new
                {
                    WebconfigId = WebconfigId
                }).ToList() ?? new List<WebConfigImage>();
                foreach (var item in data)
                {
                    item.BannerInfo = new Banner(item.BannerId);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<WebConfigImage>();
            }
        }
        private WebConfigImage Save()
        {
            if (this.WebconfigId == -1)
            {
                return Insert();
            }
            else
            {
                return Update();
            }
        }
        public WebConfigImage Insert()
        {
            try
            {
                return db.Query<WebConfigImage>("WebConfigImage_Insert", new
                {
                    WebconfigId = WebconfigId,
                    BannerId = BannerId,
                    Index = Index,
                    LinkProduct = LinkProduct,
                    ImageId = ImageId, 
                }).FirstOrDefault() ?? new WebConfigImage();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfigImage();
            }
        }
        public WebConfigImage Update()
        {
            try
            {
                return db.Query<WebConfigImage>("WebConfigImage_Update", new
                {
                    WebconfigId = WebconfigId,
                    BannerId = BannerId,
                    Index = Index,
                    LinkProduct = LinkProduct,
                    ImageId = ImageId, 
                }).FirstOrDefault() ?? new WebConfigImage();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfigImage();
            }
        } 
    }
} 