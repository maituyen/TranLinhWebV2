using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class Seeding
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public string? Note { get; set; }
        public int? TimeAuto { get; set; }
        public bool? IsStart { get; set; }
    }
}
