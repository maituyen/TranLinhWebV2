using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class Bank
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Detail { get; set; }
    }
}
