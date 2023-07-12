using Microsoft.EntityFrameworkCore;
using MyProject.Data.Entities;

namespace MyProject.Data;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Async> Asyncs { get; set; }
    public virtual DbSet<Footer> Footer { get; set; }
    public virtual DbSet<Bank> Banks { get; set; } = null!;
    public virtual DbSet<Banner> Banners { get; set; } = null!;
    public virtual DbSet<Blog> Blogs { get; set; } = null!;
    public virtual DbSet<BlogHome> BlogHomes { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Comment> Comments { get; set; } = null!;
    public virtual DbSet<Contact> Contacts { get; set; } = null!;
    public virtual DbSet<Content> Contents { get; set; } = null!;
    public virtual DbSet<Customer> Customers { get; set; } = null!;
    public virtual DbSet<CustomerHistory> CustomerHistories { get; set; } = null!;
    public virtual DbSet<Event> Events { get; set; } = null!;
    public virtual DbSet<Hastag> Hastags { get; set; } = null!;
    public virtual DbSet<History> Histories { get; set; } = null!;
    public virtual DbSet<Image> Images { get; set; } = null!;
    public virtual DbSet<KeyWord> KeyWords { get; set; } = null!;
    public virtual DbSet<KiotvietCategory> KiotvietCategories { get; set; } = null!;
    public virtual DbSet<ListShop> ListShops { get; set; } = null!;
    public virtual DbSet<Metadatum> Metadata { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public virtual DbSet<OrderSale> OrderSales { get; set; } = null!;
    public virtual DbSet<Permission> Permissions { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<ProductContent> ProductContents { get; set; } = null!;
    public virtual DbSet<ProductDetail> ProductDetails { get; set; } = null!;
    public virtual DbSet<ProductMetadatum> ProductMetadata { get; set; } = null!;
    public virtual DbSet<ProductTag> ProductTags { get; set; } = null!;
    public virtual DbSet<ProductTradeIn> ProductTradeIns { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<Seeding> Seedings { get; set; } = null!;
    public virtual DbSet<TagBlog> TagBlogs { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<WebConfig> WebConfigs { get; set; } = null!;
    public virtual DbSet<WebConfigImage> WebConfigImages { get; set; } = null!;
    public virtual DbSet<WebConfigKeyword> WebConfigKeywords { get; set; } = null!;
    public virtual DbSet<WebConfigProduct> WebConfigProducts { get; set; } = null!;
    public virtual DbSet<SiteInfo> SiteInfos { get; set; } = null!;
    public DbSet<ProductHastag> ProductHastag { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ProductHastag>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity
                    .HasOne(x => x.Product)
                    .WithMany(x => x.ProductHastags);

                entity
                    .HasOne(x => x.Hastag)
                    .WithMany(x => x.ProductHastags);
            });

        modelBuilder
            .Entity<Async>(entity =>
        {
            entity.ToTable("Async");

            entity.Property(e => e.Date).HasColumnType("datetime");
        });

        modelBuilder
           .Entity<Footer>(entity =>
           {
               entity.ToTable("Footer");
               entity.Property(e => e.CreatedAt).HasColumnType("datetime");
               entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
           });

        modelBuilder
            .Entity<Blog>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.Property(e => e.Views).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Blogs_Categories");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Blogs_Users");
        });

        modelBuilder
            .Entity<BlogHome>(entity =>
        {
            entity.ToTable("BlogHome");

            entity.HasOne(d => d.Blog)
                .WithMany(p => p.BlogHomes)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BlogHome_Blogs");
        });

        modelBuilder
            .Entity<Category>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(100);

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.Slug).HasMaxLength(500);

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasMany(d => d.Banners)
                .WithMany(p => p.Categories)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoryBanner",
                    l => l.HasOne<Banner>().WithMany().HasForeignKey("BannerId").HasConstraintName("FK_CategoryBanner_Banners"),
                    r => r.HasOne<Category>().WithMany().HasForeignKey("CategoryId").HasConstraintName("FK_CategoryBanner_Categories"),
                    j =>
                    {
                        j.HasKey("CategoryId", "BannerId");

                        j.ToTable("CategoryBanner");
                    });
        });

        modelBuilder
            .Entity<Comment>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Telephone).HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Comments_Customers");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Comments_Comments");

                entity.HasMany(d => d.Products)
                    .WithMany(p => p.Comments)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductComment",
                        l => l.HasOne<Product>().WithMany().HasForeignKey("ProductId").HasConstraintName("FK_ProductComment_Products"),
                        r => r.HasOne<Comment>().WithMany().HasForeignKey("CommentId").HasConstraintName("FK_ProductComment_Comments"),
                        j =>
                        {
                            j.HasKey("CommentId", "ProductId");

                            j.ToTable("ProductComment");
                        });
            });

        modelBuilder
            .Entity<Contact>(entity =>
        {
            entity.ToTable("Contact");

            entity.Property(e => e.Action).HasMaxLength(50);

            entity.Property(e => e.BankInstallment).HasMaxLength(50);

            entity.Property(e => e.Cmnd).HasMaxLength(500);

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.Property(e => e.Email).HasMaxLength(50);

            entity.Property(e => e.InterestRate).HasMaxLength(50);

            entity.Property(e => e.MonthInstallment).HasMaxLength(50);

            entity.Property(e => e.Phonenumber).HasMaxLength(50);

            entity.Property(e => e.RatioInstallment).HasMaxLength(50);

            entity.Property(e => e.TradeInType).HasMaxLength(50);
        });

        modelBuilder
            .Entity<Content>(entity =>
        {
            entity.HasMany(d => d.Categories)
                .WithMany(p => p.Contents)
                .UsingEntity<Dictionary<string, object>>(
                    "ContentCategory",
                    l => l.HasOne<Category>().WithMany().HasForeignKey("Categoryid").HasConstraintName("FK_ContentCategory_Categories"),
                    r => r.HasOne<Content>().WithMany().HasForeignKey("ContentId").HasConstraintName("FK_ContentCategory_Contents"),
                    j =>
                    {
                        j.HasKey("ContentId", "Categoryid");

                        j.ToTable("ContentCategory");
                    });
        });

        modelBuilder
            .Entity<Customer>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.Property(e => e.Date).HasColumnType("date");

            entity.Property(e => e.Email).HasMaxLength(50);

            entity.Property(e => e.Fullname).HasMaxLength(50);

            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder
            .Entity<CustomerHistory>(entity =>
        {
            entity.HasOne(d => d.Customer)
                .WithMany(p => p.CustomerHistories)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CustomerHistories_Customers");
        });

        modelBuilder
            .Entity<Event>(entity =>
        {
            entity.Property(e => e.End).HasColumnType("datetime");

            entity.Property(e => e.Start).HasColumnType("datetime");

            entity.HasMany(d => d.Categories)
                .WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "ListCategoryEvent",
                    l => l.HasOne<Category>().WithMany().HasForeignKey("CategoryId").HasConstraintName("FK_ListCategoryEvent_Categories"),
                    r => r.HasOne<Event>().WithMany().HasForeignKey("EventId").HasConstraintName("FK_ListCategoryEvent_Events"),
                    j =>
                    {
                        j.HasKey("EventId", "CategoryId");

                        j.ToTable("ListCategoryEvent");
                    });
        });

        modelBuilder
            .Entity<Hastag>(entity =>
        {
            entity.Property(e => e.Code).HasMaxLength(50);

            entity.Property(e => e.Name).HasMaxLength(500);

            entity.Property(e => e.Type).HasMaxLength(500);
        });

        modelBuilder.Entity<History>(entity =>
        {
            entity.ToTable("History");

            entity.Property(e => e.Datetime).HasColumnType("datetime");

            entity.Property(e => e.Ip).HasColumnName("IP");
        });

        modelBuilder
            .Entity<Image>(entity =>
        {
            entity.Property(e => e.Size).HasMaxLength(50);

            entity.HasOne(d => d.Categor)
                .WithMany(p => p.Images)
                .HasForeignKey(d => d.CategorId)
                .HasConstraintName("FK_Images_Categories");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Images_Products");
        });

        modelBuilder
            .Entity<KeyWord>(entity =>
        {
            entity.HasOne(d => d.Category)
                .WithMany(p => p.KeyWords)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_KeyWords_Categories");

            entity.HasMany(d => d.Products)
                .WithMany(p => p.Keywords)
                .UsingEntity<Dictionary<string, object>>(
                    "KeywordProduct",
                    l => l.HasOne<Product>().WithMany().HasForeignKey("ProductId").HasConstraintName("FK_KeywordProduct_Products"),
                    r => r.HasOne<KeyWord>().WithMany().HasForeignKey("KeywordId").HasConstraintName("FK_KeywordProduct_KeyWords"),
                    j =>
                    {
                        j.HasKey("KeywordId", "ProductId");

                        j.ToTable("KeywordProduct");
                    });
        });

        modelBuilder
            .Entity<KiotvietCategory>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder
            .Entity<ListShop>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder
            .Entity<Order>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Orders_Customers");
        });

        modelBuilder
            .Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.Color).HasMaxLength(50);

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.PosCode).HasMaxLength(50);

            entity.Property(e => e.Size).HasMaxLength(50);

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetails_Orders");
        });

        modelBuilder
            .Entity<OrderSale>(entity =>
        {
            entity.ToTable("OrderSale");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
        });

        modelBuilder
            .Entity<Permission>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Roles)
                .WithMany(p => p.Permistions)
                .UsingEntity<Dictionary<string, object>>(
                    "PermissionRole",
                    l => l.HasOne<Role>().WithMany().HasForeignKey("RoleId").HasConstraintName("FK_PermistionRole_Roles"),
                    r => r.HasOne<Permission>().WithMany().HasForeignKey("PermistionId").HasConstraintName("FK_PermistionRole_Permisions"),
                    j =>
                    {
                        j.HasKey("PermistionId", "RoleId").HasName("PK_PermistionRole");

                        j.ToTable("PermissionRole");
                    });
        });

        modelBuilder
            .Entity<Product>(entity =>
        {
            entity.Property(e => e.AttributesColor).HasMaxLength(50);

            entity.Property(e => e.AttributesSize).HasMaxLength(50);

            entity.Property(e => e.Capacity).HasMaxLength(50);

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.Property(e => e.Description).HasColumnType("text");

            entity.Property(e => e.KiotVietCategoryName).HasMaxLength(500);

            entity.Property(e => e.KiotVietCode).HasMaxLength(50);

            entity.Property(e => e.KiotVietFullName).HasMaxLength(500);

            entity.Property(e => e.KiotVietMasterProductId).HasMaxLength(50);

            entity.Property(e => e.KiotVietTradeMarkName).HasMaxLength(500);

            entity.Property(e => e.MasterProductId).HasMaxLength(500);

            entity.Property(e => e.Name).HasMaxLength(500);

            entity.Property(e => e.SeoSlug).HasMaxLength(100);

            entity.Property(e => e.SeoUrl).HasMaxLength(100);

            entity.Property(e => e.Tag).HasMaxLength(50);

            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Products_Users");

            entity.HasMany(d => d.Categories)
                .WithMany(p => p.ProductsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    l => l.HasOne<Category>().WithMany().HasForeignKey("CategoryId").HasConstraintName("FK_ProductCategory_Categories"),
                    r => r.HasOne<Product>().WithMany().HasForeignKey("ProductId").HasConstraintName("FK_ProductCategory_Products"),
                    j =>
                    {
                        j.HasKey("ProductId", "CategoryId");

                        j.ToTable("ProductCategory");
                    });

            entity.HasMany(d => d.Events)
                .WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoryEvent",
                    l => l.HasOne<Event>().WithMany().HasForeignKey("EventId").HasConstraintName("FK_CategoryEvent_Events"),
                    r => r.HasOne<Product>().WithMany().HasForeignKey("ProductId").HasConstraintName("FK_CategoryEvent_Products"),
                    j =>
                    {
                        j.HasKey("ProductId", "EventId");

                        j.ToTable("CategoryEvent");
                    });
        });

        modelBuilder
            .Entity<ProductContent>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.ContentId });

            entity.ToTable("ProductContent");

            entity.HasOne(d => d.Content)
                .WithMany(p => p.ProductContents)
                .HasForeignKey(d => d.ContentId)
                .HasConstraintName("FK_ProductContent_Contents");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductContents)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductContent_Products");
        });

        modelBuilder
            .Entity<ProductDetail>(entity =>
        {
            entity.Property(e => e.Code).HasMaxLength(500);

            entity.Property(e => e.Color).HasMaxLength(100);

            entity.Property(e => e.Name).HasMaxLength(500);

            entity.Property(e => e.Price).HasMaxLength(50);

            entity.Property(e => e.ProductCode).HasMaxLength(500);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductDetails_Products");
        });

        modelBuilder
            .Entity<ProductMetadatum>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.MetadataId });

            entity.HasOne(d => d.Metadata)
                .WithMany(p => p.ProductMetadata)
                .HasForeignKey(d => d.MetadataId)
                .HasConstraintName("FK_ProductMetadata_Metadata");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductMetadata)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductMetadata_Products");
        });

        modelBuilder
            .Entity<ProductTag>(entity =>
        {
            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductTags)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductTags_Products");
        });

        modelBuilder
            .Entity<ProductTradeIn>(entity =>
            {
                entity.ToTable("ProductTradeIn");

                entity.Property(e => e.Name).HasMaxLength(4000);

                entity.Property(e => e.Price).HasMaxLength(50);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductTradeIns)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ProductTradeIn_Products");
            });

        modelBuilder
            .Entity<Role>(entity =>
            {
                entity.Property(e => e.ActionCode).HasMaxLength(50);

                entity.Property(e => e.ActionName).HasMaxLength(50);
            });

        modelBuilder
            .Entity<Seeding>(entity =>
            {
                entity.ToTable("Seeding");

                entity.Property(e => e.Date).HasColumnType("datetime");
            });

        modelBuilder
            .Entity<TagBlog>(entity =>
            {
                entity.ToTable("TagBlog");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.TagBlogs)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TagBlog_Blogs");
            });

        modelBuilder
            .Entity<User>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fullname).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(500);

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasMany(d => d.Permissions)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserPermission",
                        l => l.HasOne<Permission>().WithMany().HasForeignKey("PermissionId").HasConstraintName("FK_UserPermistion_Permision"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").HasConstraintName("FK_UserPermistion_Users"),
                        j =>
                        {
                            j.HasKey("UserId", "PermissionId").HasName("PK_UserPermistion");

                            j.ToTable("UserPermission");
                        });
            });

        modelBuilder
            .Entity<WebConfig>(entity =>
            {
                entity.ToTable("WebConfig");
            });
        modelBuilder
          .Entity<SiteInfo>(entity =>
          {
              entity.ToTable("SiteInfo");
          });
        modelBuilder
            .Entity<WebConfigImage>(entity =>
            {
                entity.HasKey(e => new { e.WebconfigId, e.BannerId });

                entity.ToTable("WebConfigImage");

                entity.HasOne(d => d.Banner)
                    .WithMany(p => p.WebConfigImages)
                    .HasForeignKey(d => d.BannerId)
                    .HasConstraintName("FK_WebConfigImage_Banners");

                entity.HasOne(d => d.Webconfig)
                    .WithMany(p => p.WebConfigImages)
                    .HasForeignKey(d => d.WebconfigId)
                    .HasConstraintName("FK_WebConfigImage_WebConfig");
            });

        modelBuilder
            .Entity<WebConfigKeyword>(entity =>
            {
                entity.HasKey(e => new { e.WebconfigId, e.KeywordId });

                entity.ToTable("WebConfigKeyword");

                entity.HasOne(d => d.Keyword)
                    .WithMany(p => p.WebConfigKeywords)
                    .HasForeignKey(d => d.KeywordId)
                    .HasConstraintName("FK_WebConfigKeyword_KeyWords");

                entity.HasOne(d => d.Webconfig)
                    .WithMany(p => p.WebConfigKeywords)
                    .HasForeignKey(d => d.WebconfigId)
                    .HasConstraintName("FK_WebConfigKeyword_WebConfig");
            });

        modelBuilder
            .Entity<WebConfigProduct>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.WebConfigId });

                entity.ToTable("WebConfigProduct");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.WebConfigProducts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_WebConfigProduct_Products");

                entity.HasOne(d => d.WebConfig)
                    .WithMany(p => p.WebConfigProducts)
                    .HasForeignKey(d => d.WebConfigId)
                    .HasConstraintName("FK_WebConfigProduct_WebConfig");
            });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
