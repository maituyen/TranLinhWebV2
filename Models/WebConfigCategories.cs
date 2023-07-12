using System;
namespace MyProject.Models
{
	public class WebConfigCategories
	{
        Helpers.Database db = new Helpers.Database();
        public int CategoriesId { set; get; }
        public int WebConfigId { set; get; }
        public int Index { set; get; }
        public Category CategoryInfo { set; get; }
        public WebConfigCategories()
        {
            CategoryInfo = new Category();
        }
        public WebConfigCategories(int Id)
        {
            LoadData(GetById(Id));
        }
        public void LoadData(WebConfigCategories data)
        {
            this.CategoriesId = data.CategoriesId;
            this.WebConfigId = data.WebConfigId;
            this.Index = data.Index;
            this.CategoryInfo = new Category(this.CategoriesId);
        }
        public WebConfigCategories GetById(int Id)
        {
            try
            {
                return db.Query<WebConfigCategories>("WebConfigCategories_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new WebConfigCategories();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfigCategories();
            }
        }
        public List<WebConfigCategories> GetByWebConfigId(int WebConfigId)
        {
            try
            {
                var data = db.Query<WebConfigCategories>("WebConfigCategories_SelectByWebConfigId", new
                {
                    WebConfigId = WebConfigId
                }).ToList() ?? new List<WebConfigCategories>();
                foreach (var item in data)
                {
                    item.CategoryInfo = new Category(item.CategoriesId);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<WebConfigCategories>();
            }
        }
        private WebConfigCategories Save()
        {
            if (this.WebConfigId == -1)
            {
                return Insert();
            }
            else
            {
                return Update();
            }
        }
        public WebConfigCategories Insert()
        {
            try
            {
                return db.Query<WebConfigCategories>("WebConfigCategories_Insert", new
                {
                    CategoriesId = CategoriesId,
                    WebConfigId = WebConfigId,
                    Index = Index,
                }).FirstOrDefault() ?? new WebConfigCategories();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfigCategories();
            }
        }
        public WebConfigCategories Update()
        {
            try
            {
                return db.Query<WebConfigCategories>("WebConfigCategories_Update", new
                {
                    CategoriesId = CategoriesId,
                    WebConfigId = WebConfigId,
                    Index = Index,
                }).FirstOrDefault() ?? new WebConfigCategories();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfigCategories();
            }
        }
    }
}

