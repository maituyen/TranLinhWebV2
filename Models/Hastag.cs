using System;
using MyProject.Data.Entities;

namespace MyProject.Models
{
    public class Hastag
    {
        Helpers.Database db = new Helpers.Database();
        public int Id { get; set; } = -1;
        public string Code { get; set; }
        public string Name { get; set; }
        public int KeywordId { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public string Src { get; set; }
        public string AnchorLink { get; set; }
        public string Alt { get; set; } 
        /// <summary>
        /// 1 - Text
        /// 2 - Image
        /// 3 - Video
        /// 4 - Link
        /// 5 - Render
        /// </summary>
        public int Type { get; set; }
        public Hastag()
        {

        } 
        public Hastag(int Id)
        {
            LoadData(GetById(Id));
        }
        public Hastag(string Code)
        {
            LoadData(GetByCode(Code));
        }
        public void LoadData(Hastag data)
        {
            this.Id = data.Id;
            this.Code = data.Code;
            this.Name = data.Name;
            this.Type = data.Type;
            this.KeywordId = data.KeywordId;
            this.Width = data.Width;
            this.Height = data.Height;
            this.Src = data.Src;
            this.AnchorLink = data.AnchorLink;
            this.Alt = data.Alt; 
        }
        public Hastag GetByCode(string Code)
        {
            try
            {
                return db.Query<Hastag>("Hastags_SelectByCode", new
                {
                    Code = Code
                }).FirstOrDefault() ?? new Hastag();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Hastag();
            }
        }
        public Hastag GetById(int Id)
        {
            try
            {
                return db.Query<Hastag>("Hastags_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new Hastag();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Hastag();
            }
        }
        /// <summary>
        /// Status=-1 lấy tất cả
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        public List<Hastag> GetAll(int Type)
        {
            try
            {
                var data = db.Query<Hastag>("Hastags_SelectByType", new
                {
                    Type = Type
                }).ToList() ?? new List<Hastag>();

                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<Hastag>();
            }
        }
        public Hastag Save()
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
        private Hastag Insert()
        {
            try
            {
                return db.Query<Hastag>("Hastags_Insert", new
                {
                    Id = Id,
                    Code = Code,
                    Name = Name,
                    Type = Type,
                    KeywordId = KeywordId,
                    Width = Width,
                    Height = Height,
                    Src = Src,
                    AnchorLink = AnchorLink,
                    Alt = Alt, 
                }).FirstOrDefault() ?? new Hastag();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Hastag();
            }
        }
        private Hastag Update()
        {
            try
            {
                return db.Query<Hastag>("Hastags_Update", new
                {
                    Id = Id,
                    Code = Code,
                    Name = Name,
                    Type = Type,
                    KeywordId = KeywordId,
                    Width = Width,
                    Height = Height,
                    Src = Src,
                    AnchorLink = AnchorLink,
                    Alt = Alt,
                }).FirstOrDefault() ?? new Hastag();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Hastag();
            }
        }
        public Helpers.ReturnClient Delete()
        {
            Hastag hastag = new Hastag(Id);
            if (hastag.Id == -1)
            {
                return new Helpers.ReturnClient
                {
                    sucess = false,
                    message = "Hastag không tồn tại"
                };
            }
            db.Query<Hastag>("Hastags_Delete", new
            {
                Id = Id
            });

            Helpers.Files.Delete(hastag.Src);
            return new Helpers.ReturnClient
            {
                sucess = true,
                message = "Xóa Hastag thành công!"
            };
        }
    }
}