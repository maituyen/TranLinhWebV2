using System;
using System.Collections.Generic;

namespace MyProject.Models
{
    public partial class CustomerHistory
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string? ActionHistory { get; set; }

        public virtual Customer? Customer { get; set; }
    }
}
