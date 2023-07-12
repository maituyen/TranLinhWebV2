namespace MyProject.ViewModels;

public class FrontEndTopVm
{
    public string? Code { get; set; }
    public int? Sort { get; set; }
    public IEnumerable<WebConfigImagesVm>? WebConfigImages { get; set; }
    public IEnumerable<WebConfigKeywordsVm>? WebConfigKeywords { get; set; }

}

public class FrontEndFooterVm
{
    public string? Name { get; set; }
    public int? Sort { get; set; }
    public List<WebConfigKeywordsVm> WebConfigKeywords { get; set; } = new List<WebConfigKeywordsVm>();

}


public class WebConfigImagesVm
{
    public string? LinkImage { get; set; }
    public string? Name { get; set; }
}

public class WebConfigKeywordsVm
{
    public int? CategoryId { get; set; }
    public string? LinkImage { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
}
