using System;
using Google.Apis.PeopleService.v1.Data;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace MyProject.Models
{
	public class ProductAttributes
	{
        Helpers.Database db = new Helpers.Database();
        public int Id { set; get; } = -1;
		public int ProductId { set; get; }
        public string AttributeName { set; get; }
		public string AttributeValue { set; get; }
        public ProductAttributes()
		{
		}
        public ProductAttributes(int Id)
        {
            LoadData(GetById(Id));
        } 
        public void LoadData(ProductAttributes data)
        {
            this.Id = data.Id;
            this.ProductId = data.ProductId;
            this.AttributeName = data.AttributeName;
            this.AttributeValue = data.AttributeValue;
        } 
        public ProductAttributes GetById(int Id)
        {
            try
            {
                return db.Query<ProductAttributes>("ProductAttributes_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new ProductAttributes();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new ProductAttributes();
            }
        } 
        public List<ProductAttributes> GetByProductId(int ProductId)
        {
            try
            {
                var data = db.Query<ProductAttributes>("ProductAttributes_SelectByProductId", new
                {
                    ProductId = ProductId
                }).ToList() ?? new List<ProductAttributes>();

                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<ProductAttributes>();
            }
        }
        public ProductAttributes Save()
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
        private ProductAttributes Insert()
        {
            try
            {
                return db.Query<ProductAttributes>("ProductAttributes_Insert", new
                {
                    Id = Id,
                    ProductId = ProductId,
                    AttributeName = AttributeName,
                    AttributeValue = AttributeValue,
                }).FirstOrDefault() ?? new ProductAttributes();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new ProductAttributes();
            }
        }
        private ProductAttributes Update()
        {
            try
            {
                return db.Query<ProductAttributes>("ProductAttributes_Update", new
                {
                    Id = Id,
                    ProductId = ProductId,
                    AttributeName = AttributeName,
                    AttributeValue = AttributeValue,
                }).FirstOrDefault() ?? new ProductAttributes();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new ProductAttributes();
            }
        }
        public Helpers.ReturnClient Delete()
        {
            ProductAttributes ProductAttributes = new ProductAttributes(Id);
            if (ProductAttributes.Id == -1)
            {
                return new Helpers.ReturnClient
                {
                    sucess = false,
                    message = "ProductAttributes không tồn tại"
                };
            }
            db.Query<ProductAttributes>("ProductAttributes_Delete", new
            {
                Id = Id
            }); 
            return new Helpers.ReturnClient
            {
                sucess = true,
                message = "Xóa ProductAttributes thành công!"
            };
        }
    }
}