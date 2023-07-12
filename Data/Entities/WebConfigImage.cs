using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class WebConfigImage
    {
        public int WebconfigId { get; set; }
        public int BannerId { get; set; }
        public int? Index { get; set; }
        public string? LinkProduct { get; set; }

        public virtual Banner Banner { get; set; } = null!;
        public virtual WebConfig Webconfig { get; set; } = null!;
    }
}
