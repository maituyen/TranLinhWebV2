using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class OrderSale
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Sale { get; set; }
        public int? Money { get; set; }
    }
}
