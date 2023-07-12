using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class Content
    {
        public Content()
        {
            ProductContents = new HashSet<ProductContent>();
            Categories = new HashSet<Category>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<ProductContent> ProductContents { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
