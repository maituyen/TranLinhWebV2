
namespace MyProject.Data.Entities
{
    public partial class Banner
    {
        public Banner()
        {
            WebConfigImages = new HashSet<WebConfigImage>();
            Categories = new HashSet<Category>();
        }

        public int Id { get; set; }
        public string? LinkImage { get; set; }
        public string? Name { get; set; }
        public int? Index { get; set; }

        public virtual ICollection<WebConfigImage> WebConfigImages { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
