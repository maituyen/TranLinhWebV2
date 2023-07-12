using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MyProject.Data.Entities;
using MyProject.ViewModels;

namespace MyProject.Models
{
	public class Banner
    {
        Helpers.Database db = new Helpers.Database();
        public int Id { set; get; } = -1;
        public string? LinkImage { set; get; }
        public string? LinkImageMobile { set; get; }
        public int? Index { set; get; } = 0;
        public string? Name { set; get; }
        public string? BackgroundColor { set; get; }
        public int? Height { set; get; } = 0;
        public Banner()
        {
             
        }
        public Banner(int Id)
        {
            LoadData(GetById(Id));
        } 
        public void LoadData(Banner data)
        {
            this.Id = data.Id;
            this.LinkImage = data.LinkImage;
            this.LinkImageMobile = data.LinkImageMobile;
            this.Index = data.Index;
            this.Name = data.Name;
            this.BackgroundColor = data.BackgroundColor;
            this.Height = data.Height;
        }
        public Banner GetById(int Id)
        {
            try
            {
                return db.Query<Banner>("Banners_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new Banner();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Banner();
            }
        } 
        public List<Banner> GetAll()
        {
            try
            {
                var data= db.Query<Banner>("Banners_SelectAll").ToList() ?? new List<Banner>();
                
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<Banner>();
            }
        }
        public Banner Save()
        {
            if (this.Id == -1)
            {
                return Insert();
            }
            else
            {
                return Update();
            }
        }
        private Banner Insert()
        {
            try
            {
                return db.Query<Banner>("Banners_Insert", new
                {
                    Id = Id,
                    LinkImage = LinkImage,
                    LinkImageMobile = LinkImageMobile,
                    Index = Index,
                    Name = Name,
                    BackgroundColor = BackgroundColor,
                    Height = Height, 
                }).FirstOrDefault() ?? new Banner();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Banner();
            }
        }
        private Banner Update()
        {
            try
            {
                return db.Query<Banner>("Banners_Update", new
                {
                    Id = Id,
                    LinkImage = LinkImage,
                    LinkImageMobile = LinkImageMobile,
                    Index = Index,
                    Name = Name,
                    BackgroundColor = BackgroundColor,
                    Height = Height, 
                }).FirstOrDefault() ?? new Banner();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Banner();
            }
        }
        public Helpers.ReturnClient Delete()
        {
            Banner banner = new Banner(Id);
            if (banner.Id == -1)
            {
                return new Helpers.ReturnClient
                {
                    sucess = false,
                    message = "Banner không tồn tại"
                };
            }
            db.Query<Hastag>("Banners_Delete", new
            {
                Id = Id
            });

            Helpers.Files.Delete(banner.LinkImage);
            Helpers.Files.Delete(banner.LinkImageMobile);
            return new Helpers.ReturnClient
            {
                sucess = true,
                message = "Xóa Banner thành công!"
            };
        } 
    }
} 