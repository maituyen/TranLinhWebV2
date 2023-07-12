namespace MyProject.ViewModels.Hastag;

public class HasTagVm
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? Src { get; set; }
    public string? AnchorLink { get; set; }
    public string? Alt { get; set; }

    /// <summary>
    /// 1 - Text
    /// 2 - Image
    /// 3 - Video
    /// </summary>
    public int? Type { get; set; }
}

public class HasTagCreateVm
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? AnchorLink { get; set; }
    public string? Alt { get; set; }
    public IFormFile? File { get; set; }

    /// <summary>
    /// 1 - Text
    /// 2 - Image
    /// 3 - Video
    /// </summary>
    public int? Type { get; set; }
}