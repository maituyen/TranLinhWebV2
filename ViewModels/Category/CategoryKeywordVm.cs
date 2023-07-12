using MyProject.Data.Entities;

namespace MyProject.ViewModels.Category;

public class CategoryKeywordVm
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? FormatName { get; set; }
    public int ParentId { get; set; }
    public bool? IsShow { get; set; }
    public ICollection<Image>? Images { get; set; }
    public ICollection<KeyWord>? KeyWords { get; set; }
    public int? Status { get; set; }
    public string? Tags { get; set; }
    public string? Icon { get; set; }
    public string? Slug { get; set; }
    public int Level { get; set; }

    public List<CategoryKeywordVm> Childrens { get; set; } = new List<CategoryKeywordVm>();
}
