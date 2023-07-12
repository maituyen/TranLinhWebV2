using System;
using MyProject.Data.Entities;

namespace MyProject.Models
{
    public class WebConfigProduct
    {
        Helpers.Database db = new Helpers.Database();
        public int ProductId { set; get; }
        public int WebConfigId { set; get; }
        public int Index { set; get; } 
        public Product ProductInfo { set; get; }
        public WebConfigProduct()
        {
            ProductInfo = new Product();
        }
        public WebConfigProduct(int Id)
        {
            LoadData(GetById(Id));
        }
        public void LoadData(WebConfigProduct data)
        {
            this.ProductId = data.ProductId;
            this.WebConfigId = data.WebConfigId;
            this.Index = data.Index;
            this.ProductInfo = new Product(this.ProductId);
        }
        public WebConfigProduct GetById(int Id)
        {
            try
            {
                return db.Query<WebConfigProduct>("WebConfigProduct_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new WebConfigProduct();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfigProduct();
            }
        }
        public List<WebConfigProduct> GetByWebConfigId(int WebConfigId)
        {
            try
            {
                var data= db.Query<WebConfigProduct>("WebConfigProduct_SelectByWebConfigId", new
                {
                    WebConfigId = WebConfigId
                }).ToList() ?? new List<WebConfigProduct>(); 
                foreach (var item in data)
                { 
                    item.ProductInfo = new Product(item.ProductId);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<WebConfigProduct>();
            }
        }
        private WebConfigProduct Save()
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
        public WebConfigProduct Insert()
        {
            try
            {
                return db.Query<WebConfigProduct>("WebConfigProduct_Insert", new
                {
                    ProductId = ProductId,
                    WebConfigId = WebConfigId,
                    Index = Index, 
                }).FirstOrDefault() ?? new WebConfigProduct();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfigProduct();
            }
        }
        public WebConfigProduct Update()
        {
            try
            {
                return db.Query<WebConfigProduct>("WebConfigProduct_Update", new
                {
                    ProductId = ProductId,
                    WebConfigId = WebConfigId,
                    Index = Index,
                }).FirstOrDefault() ?? new WebConfigProduct();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new WebConfigProduct();
            }
        }
    }
}