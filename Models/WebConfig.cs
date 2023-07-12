using System;
using MyProject.Data.Entities; 

namespace MyProject.Models
{
	public class WebConfig
    {
        Helpers.Database db = new Helpers.Database();
        public int Id { set; get; } = -1;
        public string Name { set; get; }
        public string Code { set; get; }
        public int Sort { set; get; }
        public string Desciption { set; get; }
        public int Status { set; get; }
        public string Link { set; get; }
        public virtual ICollection<WebConfigImage> WebConfigImages { get; set; }
        public virtual ICollection<WebConfigProduct> WebConfigProducts { get; set; }
        public virtual ICollection<WebConfigKeyword> WebConfigKeywords { get; set; }
        public virtual ICollection<WebConfigCategories> WebConfigCategories { get; set; }

        public virtual ICollection<Banner> Images { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<KeyWord> Keywords { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public WebConfig()
        {
            WebConfigImages = new HashSet<WebConfigImage>();
            WebConfigProducts = new HashSet<WebConfigProduct>();
            WebConfigKeywords= new HashSet<WebConfigKeyword>();
            WebConfigCategories = new HashSet<WebConfigCategories>();

            Images = new HashSet<Banner>();
            Products = new HashSet<Product>();
            Keywords = new HashSet<KeyWord>();
            Categories = new HashSet<Category>();
        }
        public WebConfig(int Id)
        {
            LoadData(GetById(Id));
        } 
        public void LoadData(WebConfig data)
        {
            this.Id = data.Id;
            this.Name = data.Name;
            this.Code = data.Code;
            this.Sort = data.Sort;
            this.Desciption = data.Desciption;
            this.Status = data.Status;
            WebConfigImage webConfigImage = new WebConfigImage();
            WebConfigProduct webConfigProduct = new WebConfigProduct();
            WebConfigCategories webConfigCategory = new WebConfigCategories();
            WebConfigKeyword webConfigKeyword = new WebConfigKeyword();
            WebConfigImages = webConfigImage.GetByWebConfigId(this.Id);
            WebConfigProducts = webConfigProduct.GetByWebConfigId(Id);
            WebConfigKeywords = webConfigKeyword.GetByWebConfigId(Id);
            WebConfigCategories = webConfigCategory.GetByWebConfigId(Id);
            this.Link = data.Link;
        }
        public WebConfig GetById(int Id)
        {
            try
            {
                return db.Query<WebConfig>("WebConfig_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new WebConfig();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfig();
            }
        }
        public List<WebConfig> GetAllByCode(string Code)
        {
            try
            {
                var data = db.Query<WebConfig>("WebConfig_SelectByCode", new
                {
                    Code = Code
                }).ToList() ?? new List<WebConfig>();

                foreach (var item in data)
                {
                    WebConfigImage webConfigImage = new WebConfigImage();
                    WebConfigProduct webConfigProduct = new WebConfigProduct();
                    WebConfigKeyword webConfigKeyword = new WebConfigKeyword();
                    WebConfigCategories webConfigCategory = new WebConfigCategories();

                    item.WebConfigImages = new HashSet<WebConfigImage>();
                    item.WebConfigProducts = new HashSet<WebConfigProduct>();
                    item.WebConfigKeywords = new HashSet<WebConfigKeyword>();
                    item.WebConfigCategories = new HashSet<WebConfigCategories>();

                    item.WebConfigImages = webConfigImage.GetByWebConfigId(item.Id);
                    item.WebConfigProducts = webConfigProduct.GetByWebConfigId(item.Id);
                    item.WebConfigKeywords = webConfigKeyword.GetByWebConfigId(item.Id);
                    item.WebConfigCategories = webConfigCategory.GetByWebConfigId(item.Id);

                    foreach (var sub in item.WebConfigImages)
                    {
                        item.Images.Add(sub.BannerInfo);
                    }

                    foreach (var sub in item.WebConfigProducts)
                    {
                        item.Products.Add(sub.ProductInfo);
                    }

                    foreach (var sub in item.WebConfigKeywords)
                    {
                        item.Keywords.Add(sub.KeyWordInfo);
                    }
                    foreach (var sub in item.WebConfigCategories)
                    {
                        item.Categories.Add(sub.CategoryInfo);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<WebConfig>();  
            }
        }
        /// <summary>
        /// Status=-1 lấy tất cả
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        public List<WebConfig> GetAll(int Status=-1)
        {
            try
            {
                var data= db.Query<WebConfig>("WebConfig_SelectByStatus", new
                {
                    Status = Status
                }).ToList() ?? new List<WebConfig>();
                foreach (var item in data)
                {
                    WebConfigImage webConfigImage = new WebConfigImage();
                    WebConfigProduct webConfigProduct = new WebConfigProduct();
                    WebConfigKeyword webConfigKeyword = new WebConfigKeyword();

                    item.WebConfigImages = new HashSet<WebConfigImage>();
                    item.WebConfigProducts = new HashSet<WebConfigProduct>();
                    item.WebConfigKeywords = new HashSet<WebConfigKeyword>();

                    item.WebConfigImages = webConfigImage.GetByWebConfigId(item.Id);
                    item.WebConfigProducts = webConfigProduct.GetByWebConfigId(item.Id);
                    item.WebConfigKeywords = webConfigKeyword.GetByWebConfigId(item.Id);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<WebConfig>();
            }
        }
        private WebConfig Save()
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
        public WebConfig Insert()
        {
            try
            {
                return db.Query<WebConfig>("WebConfig_Insert", new
                {
                    Id = Id,
                    Name = Name,
                    Code = Code,
                    Sort = Sort,
                    Desciption = Desciption,
                    Status = Status, 
                }).FirstOrDefault() ?? new WebConfig();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfig();
            }
        }
        public WebConfig Update()
        {
            try
            {
                return db.Query<WebConfig>("WebConfig_Update", new
                {
                    Id = Id,
                    Name = Name,
                    Code = Code,
                    Sort = Sort,
                    Desciption = Desciption,
                    Status = Status, 
                }).FirstOrDefault() ?? new WebConfig();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfig();
            }
        } 
    }
} 