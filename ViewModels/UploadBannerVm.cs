namespace MyProject.ViewModels;

public class UploadBannerVm
{
    public int Id { get; set; }
    public IFormFile? LinkImage { get; set; }
    public int Index { get; set; }
    public string? Categories { get; set; }
    public string? Name { get; set; }
}
