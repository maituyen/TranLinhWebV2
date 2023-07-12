namespace MyProject.Data.Entities
{
    public partial class Hastag
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? KeywordId { get; set; }

        // Config some information attribute
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string? Src { get; set; }
        public string? AnchorLink { get; set; }
        public string? Alt { get; set; }

        /// <summary>
        /// 1 - Text
        /// 2 - Image
        /// 3 - Video
        /// 4 - Link
        /// </summary>
        public int? Type { get; set; }

        public virtual ICollection<ProductHastag>? ProductHastags { get; set; }

    }
}
