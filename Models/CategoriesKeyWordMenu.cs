using System;
namespace MyProject.Models
{
	public class CategoriesKeyWordMenu
	{ 
        Helpers.Database db = new Helpers.Database();
        public int Id { set; get; } = -1;
        public string Name { set; get; }
        public int CategoriesId { set; get; }
        public int KeyWordId { set; get; }
        public int ParentId { set; get; }
        public string Link { set; get; }
        public int Index { set; get; }
        public List<CategoriesKeyWordMenu> CategoriesKeyWordMenus { set; get; }
        public KeyWord KeyWordInfo { set; get; }
        public Category CategoryInfo { set; get; }
        public CategoriesKeyWordMenu()
        {

        }
        public CategoriesKeyWordMenu(int Id)
        {
            LoadData(GetById(Id));
        }
        public void LoadData(CategoriesKeyWordMenu data)
        {
            this.Id = data.Id;
            this.Name = data.Name;
            this.CategoriesId = data.CategoriesId;
            this.KeyWordId = data.KeyWordId;
            this.ParentId = data.ParentId;
            this.Link = data.Link;
            this.Index = data.Index;
            if (this.CategoriesId !=0)
                this.CategoryInfo = new Category(this.CategoriesId);
            if (this.KeyWordId != 0)
                this.KeyWordInfo = new KeyWord(this.KeyWordId);
            this.CategoriesKeyWordMenus = GetByParentId(this.Id);
        }
        public CategoriesKeyWordMenu GetById(int Id)
        {
            try
            {
                return db.Query<CategoriesKeyWordMenu>("CategoriesKeyWordMenu_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new CategoriesKeyWordMenu();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new CategoriesKeyWordMenu();
            }
        }
        public List<CategoriesKeyWordMenu> GetByCategoriesId(int CategoriesId)
        {
            try
            {
                return db.Query<CategoriesKeyWordMenu>("CategoriesKeyWordMenu_SelectByCategoriesId", new
                {
                    CategoriesId = CategoriesId
                }).ToList() ?? new List<CategoriesKeyWordMenu>();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<CategoriesKeyWordMenu>();
            }
        }
        public List<CategoriesKeyWordMenu> GetByParentId(int ParentId)
        {
            try
            {
                var data = db.Query<CategoriesKeyWordMenu>("CategoriesKeyWordMenu_SelectByParentId", new
                {
                    ParentId = ParentId
                }).ToList() ?? new List<CategoriesKeyWordMenu>();
                foreach (var item in data)
                {
                    if (item.CategoriesId != 0)
                        item.CategoryInfo = new Category(item.CategoriesId);
                    if (item.KeyWordId != 0)
                        item.KeyWordInfo = new KeyWord(item.KeyWordId);
                    item.CategoriesKeyWordMenus = GetByParentId(item.Id);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<CategoriesKeyWordMenu>();
            }
        }
        private CategoriesKeyWordMenu Save()
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
        public CategoriesKeyWordMenu Insert()
        {
            try
            {
                return db.Query<CategoriesKeyWordMenu>("CategoriesKeyWordMenu_Insert", new
                {
                    Id = Id,
                    Name = Name,
                    CategoriesId = CategoriesId,
                    KeyWordId = KeyWordId,
                    ParentId = ParentId,
                    Link = Link,
                    Index = Index,
                }).FirstOrDefault() ?? new CategoriesKeyWordMenu();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new CategoriesKeyWordMenu();
            }
        }
        public CategoriesKeyWordMenu Update()
        {
            try
            {
                return db.Query<CategoriesKeyWordMenu>("CategoriesKeyWordMenu_Update", new
                {
                    Id = Id,
                    Name = Name,
                    CategoriesId = CategoriesId,
                    KeyWordId = KeyWordId,
                    ParentId = ParentId,
                    Link = Link,
                    Index = Index,
                }).FirstOrDefault() ?? new CategoriesKeyWordMenu();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new CategoriesKeyWordMenu();
            }
        }
    }
}