using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class ProductDetail
    {
        public int Id { get; set; }
        public string? LinkImage { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Code { get; set; }
        public int? ProductId { get; set; }
        public string? Price { get; set; }
        public string? ProductCode { get; set; }
        public string? AttributesType { get; set; }
        public int? KiotVietId { get; set; }

        public virtual Product? Product { get; set; }
    }
}
