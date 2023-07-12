using System;
using MyProject.Data.Entities;

namespace MyProject.Models
{
	public class KeyWord
    {
        Helpers.Database db = new Helpers.Database();
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int ParentId { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string LinkImage { get; set; }
        public string Slug { get; set; }
        public string Blog { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<KeyWord> KeyWords { get; set; }
        public virtual ICollection<WebConfigKeyword> WebConfigKeywords { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public KeyWord()
        {
            KeyWords = new HashSet<KeyWord>();
            WebConfigKeywords = new HashSet<WebConfigKeyword>();
            Products = new HashSet<Product>();
        }
        public KeyWord(int Id)
        {
            LoadData(GetById(Id));
        }
        public void LoadData(KeyWord data)
        {
            this.Id = data.Id;
            this.Name = data.Name;
            this.CategoryId = data.CategoryId;
            this.ParentId = data.ParentId;
            this.Description = data.Description;
            this.Link = data.Link;
            this.LinkImage = data.LinkImage;
            this.Slug = data.Slug;
            this.Blog = data.Blog; 
        }
        public KeyWord GetById(int Id)
        {
            try
            {
                return db.Query<KeyWord>("KeyWord_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault()?? new KeyWord();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new KeyWord();
            }
        }
        public List<KeyWord> GetRootByCategories(int CategoryId)
        {
            try
            {
                var data = db.Query<KeyWord>("KeyWord_SelectByCategories", new
                {
                    CategoryId = CategoryId
                }).ToList() ?? new List<KeyWord>();
                foreach (var item in data)
                {
                    item.KeyWords = GetParent(item.Id);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<KeyWord>();
            }
        }
        public List<KeyWord> GetParent(int ParentId = 0)
        {
            try
            {
                var data = db.Query<KeyWord>("KeyWord_SelectByParent", new
                {
                    ParentId = ParentId
                }).ToList() ?? new List<KeyWord>();
                foreach (var item in data)
                {
                    item.KeyWords = GetParent(item.Id);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<KeyWord>();
            }
        }
        /// <summary>
        /// Status=-1 lấy tất cả
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        public List<KeyWord> GetAll(int Status = -1)
        {
            try
            {
                var data = db.Query<KeyWord>("KeyWord_SelectByStatus", new
                {
                    Status = Status
                }).ToList() ?? new List<KeyWord>();

                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<KeyWord>();
            }
        }
        private KeyWord Save()
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
        public KeyWord Insert()
        {
            try
            {
                return db.Query<KeyWord>("KeyWord_Insert", new
                {
                    Id = Id,
                    Name = Name,
                    CategoryId = CategoryId,
                    ParentId = ParentId,
                    Description = Description,
                    Link = Link,
                    LinkImage = LinkImage,
                    Slug = Slug,
                    Blog = Blog, 
                }).FirstOrDefault() ?? new KeyWord();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new KeyWord();
            }
        }
        public KeyWord Update()
        {
            try
            {
                return db.Query<KeyWord>("KeyWord_Update", new
                {
                    Id = Id,
                    Name = Name,
                    CategoryId = CategoryId,
                    ParentId = ParentId,
                    Description = Description,
                    Link = Link,
                    LinkImage = LinkImage,
                    Slug = Slug,
                    Blog = Blog,
                }).FirstOrDefault() ?? new KeyWord();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new KeyWord();
            }
        }
    }
}  
