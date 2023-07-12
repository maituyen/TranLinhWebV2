namespace MyProject.ViewModels;

public class FileUploadVm
{
    public string? Size { get; set; }
    public int Id { get; set; }
    public string? Type { get; set; }
    public IFormFile? File { get; set; }
}