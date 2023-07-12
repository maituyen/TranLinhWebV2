using MyProject.Data.Interfaces;

namespace MyProject.Data.Entities;

public class Category: IDateTracking
{
    public Category()
    {
        Blogs = new HashSet<Blog>();
        Images = new HashSet<Image>();
        KeyWords = new HashSet<KeyWord>();
        Products = new HashSet<Product>();
        Banners = new HashSet<Banner>();
        Contents = new HashSet<Content>();
        Events = new HashSet<Event>();
        ProductsNavigation = new HashSet<Product>();
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public int ParentId { get; set; }
    public bool? IsShow { get; set; }
    public bool? IsDelete { get; set; }
    public string? Description { get; set; }
    public bool? IsOld { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public bool? Pay { get; set; }
    public string? Tags { get; set; }
    public string? Icon { get; set; }
    public string? SubDescription { get; set; }
    public string? AdvertisementSmall { get; set; }
    public string? AdvertisementLarge { get; set; }
    public string? AdvertisementDetail { get; set; }
    public bool? IsNew { get; set; }
    public bool IsAccessory { get; set; }
    public string? Slug { get; set; }
    public int Level { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; }
    public virtual ICollection<Image> Images { get; set; }
    public virtual ICollection<KeyWord> KeyWords { get; set; }
    public virtual ICollection<Product> Products { get; set; }

    public virtual ICollection<Banner> Banners { get; set; }
    public virtual ICollection<Content> Contents { get; set; }
    public virtual ICollection<Event> Events { get; set; }
    public virtual ICollection<Product> ProductsNavigation { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
