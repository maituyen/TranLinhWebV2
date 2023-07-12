namespace MyProject.ViewModels.Product;
public class ProductVm
{
    public int Id { get; set; }
    public int KiotVietId { get; set; }
    public string? Name { get; set; }
    public string? KiotVietCode { get; set; }
    public string? KiotVietName { get; set; }
    public string? KiotVietFullName { get; set; }
    public string? KeySearch { get; set; }
    public string? SeoUrl { get; set; }
    public string? KiotVietMasterProductId { get; set; }
    public string? MasterProductId { get; set; }
    public int? KiotVietPrice { get; set; }
    public Data.Entities.Category? Category { get; set; }
    public int Views { get; set; }
    public bool? IsPublish { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? CreatedAt { get; set; }
}
