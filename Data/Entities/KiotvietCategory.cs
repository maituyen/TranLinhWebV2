using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class KiotvietCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? KiotvietId { get; set; }
        public int? ParentId { get; set; }
    }
}
