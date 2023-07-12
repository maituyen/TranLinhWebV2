using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class Image
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Link { get; set; }
        public string? Size { get; set; }
        public int? Index { get; set; }
        public string? LinkProduct { get; set; }
        public int? CategorId { get; set; }
        public int? ProductId { get; set; }

        public virtual Category? Categor { get; set; }
        public virtual Product? Product { get; set; }
    }
}
