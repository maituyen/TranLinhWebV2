using System;
using MyProject.Data.Entities;
using MyProject.ViewModels.Product;

namespace MyProject.Models
{
	public class Footer
    {
        Helpers.Database db = new Helpers.Database();
        public int Id { set; get; } = -1;
        public string Icon { set; get; }
        public string Name { set; get; }
        public int Sort { set; get; }
        public string Desciption { set; get; }
        public string Link { set; get; }
        public string? Code { set; get; }
        public int ParentId { set; get; }
        public virtual ICollection<Models.Footer> Footers { get; set; }
        public Footer()
        {
            Footers = new HashSet<Footer>();
        }
        public Footer(int Id)
        {
            LoadData(GetById(Id));
        } 
        public void LoadData(Footer data)
        {
            this.Id = data.Id;
            this.Icon = data.Icon;
            this.Name = data.Name;
            this.Sort = data.Sort;
            this.Desciption = data.Desciption;
            this.Link = data.Link;
            this.Code = data.Code;
            this.ParentId = data.ParentId;
        }
        public Footer GetById(int Id)
        {
            try
            {
                return db.Query<Footer>("Footers_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new Footer();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Footer();
            }
        } 
        public List<Footer> GetAll(string Code)
        {
            try
            {
                var data= db.Query<Footer>("Footer_SelectByCode", new
                {
                    Code = Code
                }).ToList() ?? new List<Footer>();
                foreach (var item in data)
                {
                    item.Footers = GetParent(item.Id);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<Footer>();
            }
        }
        public List<Footer> GetParent(int Id)
        {
            try
            {
                var data = db.Query<Footer>("Footer_SelectByParent", new
                {
                    Id = Id
                }).ToList() ?? new List<Footer>();
                foreach(var item in data)
                {
                    item.Footers = GetParent(item.Id);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<Footer>();
            }
        }
        private Footer Save()
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
        public Footer Insert()
        {
            try
            {
                return db.Query<Footer>("Footers_Insert", new
                {
                    Id = Id,
                    Icon = Icon,
                    Name = Name,
                    Sort = Sort,
                    Desciption = Desciption,
                    Link = Link,
                    Code = Code,
                    ParentId = ParentId,
                }).FirstOrDefault() ?? new Footer();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Footer();
            }
        }
        public Footer Update()
        {
            try
            {
                return db.Query<Footer>("Footers_Update", new
                {
                    Id = Id,
                    Icon = Icon,
                    Name = Name,
                    Sort = Sort,
                    Desciption = Desciption,
                    Link = Link,
                    Code = Code,
                    ParentId = ParentId,
                }).FirstOrDefault() ?? new Footer();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Footer();
            }
        } 
    }
} 