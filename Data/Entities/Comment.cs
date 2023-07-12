namespace MyProject.Data.Entities;

public partial class Comment
{
    public Comment()
    {
        InverseParent = new HashSet<Comment>();
        Products = new HashSet<Product>();
    }

    public int Id { get; set; }
    public int? Vote { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? CustomerId { get; set; }
    public bool? IsShow { get; set; }
    public string? Telephone { get; set; }
    public string? Name { get; set; }
    public int? ParentId { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual Comment? Parent { get; set; }
    public virtual ICollection<Comment> InverseParent { get; set; }

    public virtual ICollection<Product> Products { get; set; }
}
