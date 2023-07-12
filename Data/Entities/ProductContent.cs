using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class ProductContent
    {
        public int ProductId { get; set; }
        public int ContentId { get; set; }
        public int? Status { get; set; }
        public int? Sort { get; set; }

        public virtual Content Content { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
