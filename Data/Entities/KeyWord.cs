using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class KeyWord
    {
        public KeyWord()
        {
            WebConfigKeywords = new HashSet<WebConfigKeyword>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public int? ParentId { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? LinkImage { get; set; }
        public string? Slug { get; set; }
        public string? Blog { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<WebConfigKeyword> WebConfigKeywords { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
