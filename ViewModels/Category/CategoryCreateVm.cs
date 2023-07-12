namespace MyProject.ViewModels.Category;

public class CategoryCreateVm
{
    public string? Name { get; set; }
    public string? SubDescription { get; set; }
    public int ParentId { get; set; }
    public int Level { get; set; } = 1;
    public int Status { get; set; }
    public bool IsShow { get; set; }
    public bool IsOld { get; set; }
}
