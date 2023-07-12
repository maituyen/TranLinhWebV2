using MyProject.ViewModels.Product;

namespace MyProject.ViewModels
{
    public class WebConfigVm
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int Id { get; set; }
        public int? Sort { get; set; }

        public IEnumerable<WebConfigProductsVm> WebConfigProducts { get; set; } = new List<WebConfigProductsVm>();
        public IEnumerable<WebConfigKeywordsVm> WebConfigKeywords { get; set; } = new List<WebConfigKeywordsVm>();
    }

    public class WebConfigProductsVm
    {
        public ProductConfigVm? Product { get; set; }
    }
}
