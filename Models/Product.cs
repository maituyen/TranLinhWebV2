using System;
using MyProject.Data.Entities;
using MyProject.ViewModels;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MyProject.Models
{
    public class Product
	{
        Helpers.Database db = new Helpers.Database();
        public int Id { set; get; } = -1;
        public int VariantID { set; get; } = 0;
        public int HasVariant { set; get; } = 0;
        public string? Code { set; get; }
        public int CategoriesId { set; get; }
        public string? Image { set; get; }
        public string? BigImage { set; get; }
        public string Name { set; get; }
        public string? Describe { set; get; }
        public string? SeoUrl { set; get; }
        public int Views { set; get; }
        public int IsPublish { set; get; }
        public string? MetaTitle { set; get; }
        public string? MetaDesc { set; get; }
        public int IsNews { set; get; }
        public int IsAccessory { set; get; }
        public decimal? EntryPrice { set; get; }
        public decimal? PriceSales { set; get; }
        public decimal? PriceSalesOff { set; get; }
        public decimal? SubsidyPrice { set; get; }
        public decimal? Prepay { set; get; }
        public string? AttributesColor { set; get; }
        public string? AttributesSize { set; get; }
        public string? AttributesType { set; get; }
        public DateTime CreatedAt { set; get; }
        public DateTime UpdatedAt { set; get; }
        public string Type { set; get; }
        public decimal KiotVietId { set; get; }

        public virtual Category Category { get; set; }
        public virtual User User { get; set; } 

        public virtual List<Models.ProductImages> Images { get; set; }
        public virtual List<Models.ProductAttributes> Attributes { get; set; }
        public virtual List<Models.Product> Variants { get; set; }
        public virtual List<Models.Hastag> Hastags { get; set; } 

        public virtual ICollection<ProductContent> ProductContents { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ProductMetadatum> ProductMetadata { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<ProductTradeIn> ProductTradeIns { get; set; }
        public virtual ICollection<Product> ProductProducts { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<KeyWord> Keywords { get; set; }
        public Product()
        {
            Images = new List<Models.ProductImages>();
            Attributes = new List<Models.ProductAttributes>();
            Variants = new List<Models.Product>();
            Hastags = new List<Hastag>();
            //ProductContents = new HashSet<ProductContent>();
            //ProductDetails = new HashSet<ProductDetail>();
            //ProductMetadata = new HashSet<ProductMetadatum>();
            //ProductTags = new HashSet<ProductTag>();
            //ProductTradeIns = new HashSet<ProductTradeIn>();
            //ProductProducts = new HashSet<Product>();
            //Categories = new HashSet<Category>();
            //Comments = new HashSet<Comment>();
            //Events = new HashSet<Event>();
            //Keywords = new HashSet<KeyWord>();
            //ProductHastags = new HashSet<ProductHastag>();
        }
        public Product(int Id)
        {
            LoadData(GetById(Id));
        }
        public Product(string SeoUrl)
        {
            LoadData(GetById(Id));
        }
        public void LoadData(Product data)
        {
            this.Id = data.Id;
            this.VariantID = data.VariantID;
            this.Code = data.Code;
            this.CategoriesId = data.CategoriesId;
            this.Image = data.Image;
            this.BigImage = data.BigImage;
            this.Name = data.Name;
            this.Describe = data.Describe;
            this.SeoUrl = data.SeoUrl;
            this.Views = data.Views;
            this.IsPublish = data.IsPublish;
            this.MetaTitle = data.MetaTitle;
            this.MetaDesc = data.MetaDesc;
            this.IsNews = data.IsNews;
            this.IsAccessory = data.IsAccessory;
            this.EntryPrice = data.EntryPrice;
            this.PriceSales = data.PriceSales;
            this.PriceSalesOff = data.PriceSalesOff;
            this.SubsidyPrice = data.SubsidyPrice;
            this.Prepay = data.Prepay;
            this.AttributesColor = data.AttributesColor;
            this.AttributesSize = data.AttributesSize;
            this.AttributesType = data.AttributesType;
            this.CreatedAt = data.CreatedAt;
            this.UpdatedAt = data.UpdatedAt;
            this.Type = data.Type;
            this.KiotVietId = data.KiotVietId;
            ProductImages productImages = new ProductImages();
            ProductAttributes productAttributes = new ProductAttributes();
            ProductHastag productHastag = new ProductHastag();
             

            Attributes = productAttributes.GetByProductId(this.Id);
            Images = productImages.GetByProductId(this.Id);
            Variants = GetAll(Type, this.Id);
            var hastags = productHastag.GetByProductId(this.Id);
            foreach (var item in hastags)
            {
                this.Hastags.Add(new Models.Hastag(item.HastagsId));
            }
        }
        public Product GetById(int Id)
        {
            try
            {
                return db.Query<Product>("Product_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new Product();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Product();
            }
        } 
        public List<Product> GetAll(string Type,int ProductId)
        {
            try
            {
                var data= db.Query<Product>("Product_SelectAll",
                    new
                    {
                        Type= Type,
                        VariantID= ProductId
                    }
                    ).ToList() ?? new List<Product>();
                ProductImages productImages = new ProductImages();
                ProductAttributes productAttributes = new ProductAttributes();
                foreach (var item in data)
                {
                    //item.Images = new List<Models.ProductImages>();
                    //item.Attributes = new List<Models.ProductAttributes>();
                    item.Variants = new List<Models.Product>();

                    //item.Attributes = productAttributes.GetByProductId(item.Id);
                    //item.Images = productImages.GetByProductId(item.Id);
                    if (item.HasVariant == 1)
                        item.Variants = GetAll(Type, item.Id);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<Product>();
            }
        }
        public List<Product> SearchByKey(string KeyWord)
        {
            try
            {
                try
                {
                    return db.Query<Product>("Product_SelectByKeyWord", new
                    {
                        KeyWord = KeyWord
                    }).ToList() ?? new List<Product>();
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new List<Product>();
                }
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<Product>();
            }
        }
        public Product Save()
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
        private Product Insert()
        {
            try
            {
                return db.Query<Product>("Product_Insert", new
                {
                    Id = Id,
                    VariantID = VariantID,
                    Code = Code,
                    CategoriesId = CategoriesId,
                    Image = Image,
                    BigImage = BigImage,
                    Name = Name,
                    Describe = Describe,
                    SeoUrl = SeoUrl,
                    Views = Views,
                    IsPublish = IsPublish,
                    MetaTitle = MetaTitle,
                    MetaDesc = MetaDesc, 
                    EntryPrice = EntryPrice,
                    PriceSales = PriceSales,
                    PriceSalesOff = PriceSalesOff,
                    SubsidyPrice = SubsidyPrice,
                    Prepay = Prepay,
                    AttributesColor = AttributesColor,
                    AttributesSize = AttributesSize,
                    AttributesType = AttributesType,
                    CreatedAt = CreatedAt,
                    UpdatedAt = UpdatedAt,
                    Type=Type,
                    KiotVietId=KiotVietId,
                    HasVariant=HasVariant
                }).FirstOrDefault() ?? new Product();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Product();
            }
        }
        private Product Update()
        {
            try
            {
                return db.Query<Product>("Product_Update", new
                {
                    Id = Id,
                    VariantID = VariantID,
                    Code = Code,
                    CategoriesId = CategoriesId,
                    Image = Image,
                    BigImage = BigImage,
                    Name = Name,
                    Describe = Describe,
                    SeoUrl = SeoUrl,
                    Views = Views,
                    IsPublish = IsPublish,
                    MetaTitle = MetaTitle,
                    MetaDesc = MetaDesc, 
                    EntryPrice = EntryPrice,
                    PriceSales = PriceSales,
                    PriceSalesOff = PriceSalesOff,
                    SubsidyPrice = SubsidyPrice,
                    Prepay = Prepay,
                    AttributesColor = AttributesColor,
                    AttributesSize = AttributesSize,
                    AttributesType = AttributesType,
                    CreatedAt = CreatedAt,
                    UpdatedAt = UpdatedAt,
                    Type = Type,
                    KiotVietId = KiotVietId,
                    HasVariant = HasVariant
                }).FirstOrDefault() ?? new Product();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Product();
            }
        }
    }
}