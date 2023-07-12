using System;
using MyProject.Models;
using MyProject.Models.Admin;

namespace MyProject.Models.Admin
{
	public class Menu
	{
        Helpers.Database db = new Helpers.Database();
        public int Id { set; get; }
        public string Icon { set; get; }
        public string PageName { set; get; }
        public string Path { set; get; }
        public int ParentId { set; get; }
        public bool isShow { set; get; } = true;
        public virtual ICollection<Menu> Menus { get; set; }
        public Menu()
        {
            Menus = new HashSet<Menu>();
        }
        public Menu(int Id)
        {
            LoadData(GetById(Id));
        }
        public void LoadData(Menu data)
        {
            this.Id = data.Id;
            this.Icon = data.Icon;
            this.PageName = data.PageName;
            this.Path = data.Path;
            this.ParentId = data.ParentId;
        }
        public Menu GetById(int Id)
        {
            try
            {
                return db.Query<Menu>("Menus_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new Menu();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Menu();
            }
        } 
        public List<Menu> GetParent(int Id)
        {
            try
            {
                var data = db.Query<Menu>("Menus_SelectByParentId", new
                {
                    Id = Id
                }).ToList() ?? new List<Menu>();
                foreach(var item in data)
                {
                    item.Menus = new HashSet<Menu>();
                    item.Menus = GetParent(item.Id);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<Menu>();
            }
        }
        private Menu Save()
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
        public Menu Insert()
        {
            try
            {
                return db.Query<Menu>("Menus_Insert", new
                {
                   
                }).FirstOrDefault() ?? new Menu();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Menu();
            }
        }
        public Menu Update()
        {
            try
            {
                return db.Query<Menu>("Menus_Update", new
                {
                    Id = Id,
                    Icon = Icon,
                    PageName = PageName,
                    Path = Path,
                    ParentId = ParentId,
                }).FirstOrDefault() ?? new Menu();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Menu();
            }
        }
    }
}