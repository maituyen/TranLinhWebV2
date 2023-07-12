using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class TagBlog
    {
        public int Id { get; set; }
        public int? BlogId { get; set; }
        public int? BlogTagId { get; set; }

        public virtual Blog? Blog { get; set; }
    }
}
