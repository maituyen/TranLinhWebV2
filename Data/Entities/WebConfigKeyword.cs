using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class WebConfigKeyword
    {
        public int WebconfigId { get; set; }
        public int KeywordId { get; set; }
        public int? Index { get; set; }

        public virtual KeyWord Keyword { get; set; } = null!;
        public virtual WebConfig Webconfig { get; set; } = null!;
    }
}
