namespace MyProject.ViewModels.Category;

public class CategoryVm
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? SubDescription { get; set; }
    public string? Slug { get; set; }
    public string? Icon { get; set; }
}

public class CategoryAccessoryVm
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Note { get; set; }
    public int? Level { get; set; }
}

public class CategoryOldVm
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Note { get; set; }
    public int? Level { get; set; }
}

