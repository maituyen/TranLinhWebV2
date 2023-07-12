namespace MyProject.Data.Entities
{
    public partial class Product
    {
        public Product()
        {
            Images = new HashSet<Image>();
            ProductContents = new HashSet<ProductContent>();
            ProductDetails = new HashSet<ProductDetail>();
            ProductMetadata = new HashSet<ProductMetadatum>();
            ProductTags = new HashSet<ProductTag>();
            ProductTradeIns = new HashSet<ProductTradeIn>();
            WebConfigProducts = new HashSet<WebConfigProduct>();
            Categories = new HashSet<Category>();
            Comments = new HashSet<Comment>();
            Events = new HashSet<Event>();
            Keywords = new HashSet<KeyWord>();
            ProductHastags = new HashSet<ProductHastag>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsPublish { get; set; }
        public double? Sale { get; set; }
        public int? Count { get; set; }
        public int? UserId { get; set; }
        public string? Promotion { get; set; }
        public int? Vote { get; set; }
        public int? KiotVietPrice { get; set; }
        public int? Prepay { get; set; }
        public int? TheFirmId { get; set; }
        public string? KiotVietName { get; set; }
        public string? BundleOffer { get; set; }
        public string? Blog { get; set; }
        public string? Annotate { get; set; }
        public bool? IsOld { get; set; }
        public string? KeySearch { get; set; }
        public int? Status { get; set; }
        public int? EntryPrice { get; set; }
        public int? SubsidyPrice { get; set; }
        public int? CategoryId { get; set; }
        public string MasterProductId { get; set; } = null!;
        public string? Capacity { get; set; }
        public string? Tag { get; set; }
        public bool? IsProjectOld { get; set; }
        public int KiotVietId { get; set; }
        public int KiotVietCategoryId { get; set; }
        public string? KiotVietCategoryName { get; set; }
        public string? KiotVietMasterProductId { get; set; }
        public string? KiotVietFullName { get; set; }
        public string? KiotVietTradeMarkName { get; set; }
        public string? KiotVietCode { get; set; }
        public string? SeoUrl { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoName { get; set; }
        public string? SeoSlug { get; set; }
        public int Views { get; set; }
        public string? AttributesColor { get; set; }
        public string? AttributesSize { get; set; }
        public string? AttributesType { get; set; }
        public string? Image { get; set; }
        public bool IsDelete { get; set; }
        public virtual Category? Category { get; set; }
        public virtual User? User { get; set; }


        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<ProductContent> ProductContents { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ProductMetadatum> ProductMetadata { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<ProductTradeIn> ProductTradeIns { get; set; }
        public virtual ICollection<WebConfigProduct> WebConfigProducts { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<KeyWord> Keywords { get; set; }
        public virtual ICollection<ProductHastag> ProductHastags { get; set; }
    }
}
