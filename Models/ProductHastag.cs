using System; 
namespace MyProject.Models
{
	public class ProductHastag
    {
        Helpers.Database db = new Helpers.Database();
        public int Id { set; get; }
		public int ProductId {set;get;}
        public int HastagsId { set; get; }
        public Hastag HastagInfo { set; get; }
        public ProductHastag()
		{
		} 
        public ProductHastag(int Id)
        {
            LoadData(GetById(Id));
        }  
        public void LoadData(ProductHastag data)
        {
            this.Id = data.Id;
            this.ProductId = data.ProductId;
            this.HastagsId = data.HastagsId;
            this.HastagInfo = new Hastag(this.HastagsId);
        } 
        public ProductHastag GetById(int Id)
        {
            try
            {
                return db.Query<ProductHastag>("ProductHastags_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new ProductHastag();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new ProductHastag();
            }
        } 
        public List<ProductHastag> GetByProductId(int ProductId)
        {
            try
            {
                var data = db.Query<ProductHastag>("ProductHastags_SelectByProductId", new
                {
                    ProductId = ProductId
                }).ToList() ?? new List<ProductHastag>();
                foreach (var item in data)
                {
                    item.HastagInfo = new Hastag(item.HastagsId);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<ProductHastag>();
            }
        }
        public ProductHastag Save()
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
        private ProductHastag Insert()
        {
            try
            {
                return db.Query<ProductHastag>("ProductHastags_Insert", new
                {
                    Id = Id,
                    ProductId = ProductId,
                    HastagsId = HastagsId,
                }).FirstOrDefault() ?? new ProductHastag();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new ProductHastag();
            }
        }
        private ProductHastag Update()
        {
            try
            {
                return db.Query<ProductHastag>("ProductHastags_Update", new
                {
                    Id = Id,
                    ProductId = ProductId,
                    HastagsId = HastagsId,
                }).FirstOrDefault() ?? new ProductHastag();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new ProductHastag();
            }
        }
        public Helpers.ReturnClient Delete()
        {
            ProductHastag ProductHastag = new ProductHastag(Id);
            if (ProductHastag.Id == -1)
            {
                return new Helpers.ReturnClient
                {
                    sucess = false,
                    message = "ProductHastag không tồn tại"
                };
            }
            db.Query<ProductHastag>("ProductHastags_Delete", new
            {
                Id = Id
            }); 
            return new Helpers.ReturnClient
            {
                sucess = true,
                message = "Xóa ProductHastag thành công!"
            };
        }
    }
} 