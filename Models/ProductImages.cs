using System;
using Google.Apis.PeopleService.v1.Data;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace MyProject.Models
{
	public class ProductImages
	{
        Helpers.Database db = new Helpers.Database();
		public int Id { set; get; }
        public string Image { set; get; }
        public string BigImage { set; get; }
        public string Caption { set; get; }
        public int ProductId { set; get; }
        public ProductImages()
		{
		}

        public ProductImages(int Id)
        {
            LoadData(GetById(Id));
        } 
        public void LoadData(ProductImages data)
        {
            this.Id = data.Id;
            this.Image = data.Image;
            this.BigImage = data.BigImage;
            this.Caption = data.Caption;
            this.ProductId = data.ProductId;
        } 
        public ProductImages GetById(int Id)
        {
            try
            {
                return db.Query<ProductImages>("ProductImages_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new ProductImages();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new ProductImages();
            }
        }
        public List<ProductImages> GetByProductId(int ProductId)
        {
            try
            {
                return db.Query<ProductImages>("ProductImages_SelectProductId", new
                {
                    ProductId = ProductId
                }).ToList() ?? new List<ProductImages>();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<ProductImages>();
            }
        } 
        public ProductImages Save()
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
        private ProductImages Insert()
        {
            try
            {
                return db.Query<ProductImages>("ProductImages_Insert", new
                {
                    Id = Id,
                    Image = Image,
                    BigImage = BigImage,
                    Caption = Caption,
                    ProductId = ProductId,
                }).FirstOrDefault() ?? new ProductImages();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new ProductImages();
            }
        }
        private ProductImages Update()
        {
            try
            {
                return db.Query<ProductImages>("ProductImages_Update", new
                {
                    Id = Id,
                    Image = Image,
                    BigImage = BigImage,
                    Caption = Caption,
                    ProductId = ProductId,
                }).FirstOrDefault() ?? new ProductImages();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new ProductImages();
            }
        }
        public Helpers.ReturnClient Delete()
        {
            ProductImages ProductImages = new ProductImages(Id);
            if (ProductImages.Id == -1)
            {
                return new Helpers.ReturnClient
                {
                    sucess = false,
                    message = "ProductImages không tồn tại"
                };
            }
            db.Query<ProductImages>("ProductImages_Delete", new
            {
                Id = Id
            });

            Helpers.Files.Delete(ProductImages.Image);
            Helpers.Files.Delete(ProductImages.BigImage);
            return new Helpers.ReturnClient
            {
                sucess = true,
                message = "Xóa ProductImages thành công!"
            };
        }
        public Helpers.ReturnClient Delete(int ProductId)
        {
            ProductImages ProductImages = new ProductImages();

            foreach (var item in ProductImages.GetByProductId(ProductId))
            {
                Helpers.Files.Delete(item.Image);
                Helpers.Files.Delete(item.BigImage);
            }
            db.Query<ProductImages>("ProductImages_DeleteByProductId", new
            {
                Id = Id
            });

            
            return new Helpers.ReturnClient
            {
                sucess = true,
                message = "Xóa ProductImages thành công!"
            };
        }
    }
}