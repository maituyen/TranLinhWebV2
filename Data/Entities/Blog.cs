
using MyProject.Data.Interfaces;

namespace MyProject.Data.Entities;
public class Blog : IDateTracking
{
    public Blog()
    {
        BlogHomes = new HashSet<BlogHome>();
        TagBlogs = new HashSet<TagBlog>();
    }

    public int Id { get; set; }
    public int? CategoryId { get; set; }
    public string? Title { get; set; }
    public string? Thumbnail { get; set; }
    public string? Description { get; set; }
    public int? UserId { get; set; }
    public string? Body { get; set; }
    public bool? IsPublish { get; set; }
    public bool? IsHot { get; set; }
    public int? Views { get; set; }

    public virtual Category? Category { get; set; }
    public virtual User? User { get; set; }
    public virtual ICollection<BlogHome> BlogHomes { get; set; }
    public virtual ICollection<TagBlog> TagBlogs { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
