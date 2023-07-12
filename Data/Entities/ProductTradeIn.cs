using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class ProductTradeIn
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Price { get; set; }
        public int? ProductId { get; set; }
        public int? Status { get; set; }

        public virtual Product? Product { get; set; }
    }
}
