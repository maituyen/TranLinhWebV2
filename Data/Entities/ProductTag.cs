namespace MyProject.Data.Entities
{
    public partial class ProductTag
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? TagProductId { get; set; }

        public virtual Product? Product { get; set; }
    }
}
