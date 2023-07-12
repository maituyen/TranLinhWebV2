using MyProject.Data.Entities;

namespace MyProject.ViewModels.Product;

public class ProductConfigVm
{
    //Invalid column name 'Alt'.
//Invalid column name 'AnchorLink'.
//Invalid column name 'Height'.
//Invalid column name 'Src'.
//Invalid column name 'Width'.

    public int ProductId { get; set; }
    public int Id { get; set; }
    public int WebConfigId { get; set; }
    public string? Name { get; set; }
    public double? Sale { get; set; }
    public double? PriceSale { get; set; }
    public double? PriceSaleLast { get; set; }
    public double? FormatPrepay { get; set; }
    public int? KiotVietPrice { get; set; }
    public int? FormatPrice { get; set; }
    public int? Prepay { get; set; }
    public string? Annotate { get; set; }
    public string? FormatName { get; set; }
    public string? SeoUrl { get; set; }
    public string? AdvertisementDetail { get; set; }
    public string? AdvertisementLarge { get; set; }
    public string? AdvertisementSmall { get; set; }
    public IEnumerable<Event> Events { get; set; } = new List<Event>();
    public IEnumerable<Event> ProductEvents { get; set; } = new List<Event>();
    public IEnumerable<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

}
