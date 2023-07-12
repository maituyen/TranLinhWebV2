using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class BlogHome
    {
        public int Id { get; set; }
        public int? BlogId { get; set; }
        public int NumericOrder { get; set; }
        public int? Status { get; set; }

        public virtual Blog? Blog { get; set; }
    }
}
