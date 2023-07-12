using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class WebConfig
    {
        public WebConfig()
        {
            WebConfigImages = new HashSet<WebConfigImage>();
            WebConfigKeywords = new HashSet<WebConfigKeyword>();
            WebConfigProducts = new HashSet<WebConfigProduct>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? Sort { get; set; }
        public string? Desciption { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<WebConfigImage> WebConfigImages { get; set; }
        public virtual ICollection<WebConfigKeyword> WebConfigKeywords { get; set; }
        public virtual ICollection<WebConfigProduct> WebConfigProducts { get; set; }
    }
}
