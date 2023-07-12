using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public double? Quantity { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public int? Status { get; set; }
        public string? PosCode { get; set; }
        public int? OrderId { get; set; }
        public double? PriceOld { get; set; }
        public int RowId { get; set; }

        public virtual Order? Order { get; set; }
    }
}
