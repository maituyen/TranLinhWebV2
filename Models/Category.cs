using System;
using System.Collections.Generic; 
using System.Xml.Linq;
using MyProject.Data.Entities;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Microsoft.AspNetCore.SignalR;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using SlugGenerator;

namespace MyProject.Models
{
    public class Category
    {
        Helpers.Database db = new Helpers.Database();
        public int Id { get; set; } = -1;
        public string? Icon { get; set; }
        public string? Image { get; set; }
        public string? BigImage { get; set; }
        public string Name { get; set; }
        public string NameSEO { get; set; }
        public int ParentId { get; set; }
        public int IsShow { get; set; }
        public int IsDelete { get; set; }
        public string? SubDescription { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public string SeoUrl { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal KiotVietId { get; set; }
        
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
         
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<KeyWord> KeyWords { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Banner> Banners { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Product> ProductsNavigation { get; set; } 
        public Category()
        {
            Categories = new HashSet<Category>();
            Blogs = new HashSet<Blog>();
            Images = new HashSet<Image>();
            KeyWords = new HashSet<KeyWord>();
            Products = new HashSet<Product>();
            Banners = new HashSet<Banner>();
            Contents = new HashSet<Content>();
            Events = new HashSet<Event>();
            ProductsNavigation = new HashSet<Product>();
        }
        public Category(int Id)
        {
            LoadData(GetById(Id));
        }
        public Category(string url)
        {
            LoadData(GetBySlug(url));
        }
        public void LoadData(Category data)
        {
            this.Id = data.Id;
            this.Icon = data.Icon;
            this.Image = data.Image;
            this.BigImage = data.BigImage;
            this.Name = data.Name;
            this.NameSEO = data.NameSEO;
            this.ParentId = data.ParentId;
            this.IsShow = data.IsShow;
            this.IsDelete = data.IsDelete;
            this.SubDescription = data.SubDescription;
            this.Description = data.Description;
            this.Status = data.Status;
            this.SeoUrl = data.SeoUrl;
            this.Type = data.Type;
            this.CreatedAt = data.CreatedAt;
            this.UpdatedAt = data.UpdatedAt;
            this.KiotVietId = data.KiotVietId; 
        }
        public Category GetById(int Id)
        {
            try
            {
                return db.Query<Category>("Category_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new Category();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Category();
            }
        }
        public Category GetByKiotViet(int KiotVietId)
        {
            try
            {
                return db.Query<Category>("Category_SelectByKiotVietId", new
                {
                    KiotVietId = KiotVietId
                }).FirstOrDefault() ?? new Category();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Category();
            }
        }
        public List<Category> GetByParent(int ParentId)
        {
            try
            {
                var data= db.Query<Category>("Category_SelectByParentId", new
                {
                    ParentId = ParentId
                }).ToList() ?? new List<Category>();
                foreach (var item in data)
                {
                    item.Categories = new List<Category>();
                    item.Categories = GetByParent(item.Id);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<Category>();
            }
        }
        public Category GetRootCategory(Models.Category data)
        {
            if (data.ParentId == 0)
            {
                return data;
            }
            else
            {
              return  GetRootCategory(new Models.Category(data.ParentId)); 
            }
        }
        public Category GetBySlug(string Slug)
        {
            try
            {
                return db.Query<Category>("Categories_GetByUrl", new
                {
                    Slug = Slug
                }).FirstOrDefault() ?? new Category();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Category();
            }
        }
        
        public List<Category> SearchByKey(string KeyWord)
        {
            try
            {
                try
                {
                    return db.Query<Category>("Category_SelectByKeyWord", new
                    {
                        KeyWord = KeyWord
                    }).ToList() ?? new List<Category>();
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new List<Category>();
                }
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<Category>();
            }
        }
        public List<Category> GetByParent(int ParentId,string Type)
        {
            var data = db.Query<Category>("Category_SelectByParent", new
            {
                ParentId = ParentId,
                Type = Type
            }).ToList();
            foreach (var item in data)
            {
                item.Categories = GetByParent(item.Id, Type);
            }
            return data;
        }
        public List<Category> Menu()
        {
            List<Category> categories = new List<Category>();
            List<CategoriesKeyWordMenu> categoriesKeyWordMenus = new List<CategoriesKeyWordMenu>();
            List<CategoriesKeyWordMenu> categoriesKeyWordMenuParent = new List<CategoriesKeyWordMenu>();
            CategoriesKeyWordMenu categoriesKeyWordMenu = new CategoriesKeyWordMenu();
            List<KeyWord> keyWords = new List<KeyWord>();
            categories = GetByParent(0,"product");
            Models.KeyWord keyWord = new Models.KeyWord(); 
            foreach (var item in categories)
            {
                var categories_menu = categoriesKeyWordMenu.GetByCategoriesId(item.Id);
                foreach (var sub in categories_menu)
                {
                    keyWord = new Models.KeyWord
                    {
                        Name = sub.Name,
                        Link = sub.Link,
                    };
                    var categories_menu_parent = categoriesKeyWordMenu.GetByParentId(sub.Id);
                    foreach (var sub_parent in categories_menu_parent)
                    {
                        if (sub_parent.KeyWordId != 0)
                            keyWord.KeyWords.Add(new KeyWord(sub_parent.KeyWordId));
                        else
                            keyWord.KeyWords.Add(new Models.KeyWord
                            {
                                Name = sub_parent.Name,
                                Link = sub_parent.Link,
                            });
                    }
                    item.KeyWords.Add(keyWord);
                }
            } 
            return categories;
        }
        public Category Save()
        {
            if (this.Id == -1)
            {
                return Insert();
            }else
            {
                return Update();
            }
        }
        private Category Insert()
        {
            try
            {
                return db.Query<Category>("Categories_Insert", new
                {
                    Id = Id,
                    Icon = Icon,
                    Image = Image,
                    BigImage = BigImage,
                    Name = Name,
                    NameSEO = NameSEO,
                    ParentId = ParentId,
                    IsShow = IsShow,
                    IsDelete = IsDelete,
                    SubDescription = SubDescription,
                    Description = Description,
                    Status = Status,
                    SeoUrl = SeoUrl,
                    Type = Type,
                    CreatedAt = CreatedAt,
                    UpdatedAt = UpdatedAt,
                    KiotVietId = KiotVietId, 
                }).FirstOrDefault() ?? new Category();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Category();
            }
        }
        private Category Update()
        {
            try
            {
                return db.Query<Category>("Categories_Update", new
                {
                    Id = Id,
                    Icon = Icon,
                    Image = Image,
                    BigImage = BigImage,
                    Name = Name,
                    NameSEO = NameSEO,
                    ParentId = ParentId,
                    IsShow = IsShow,
                    IsDelete = IsDelete,
                    SubDescription = SubDescription,
                    Description = Description,
                    Status = Status,
                    SeoUrl = SeoUrl,
                    Type = Type,
                    CreatedAt = CreatedAt,
                    UpdatedAt = UpdatedAt,
                    KiotVietId = KiotVietId,
                }).FirstOrDefault() ?? new Category();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Category();
            }
        }
        public Helpers.ReturnClient Delete()
        {
            Category Category = new Category(Id);
            if (Category.Id == -1)
            {
                return new Helpers.ReturnClient
                {
                    sucess = false,
                    message = "Danh mục không tồn tại"
                };
            }
            var i = db.Execute("Categories_Delete", new
            {
                Id = Id
            });
            return new Helpers.ReturnClient
            {
                sucess = true,
                message = "Xóa Hastag thành công!"
            };
        }
    }
}
