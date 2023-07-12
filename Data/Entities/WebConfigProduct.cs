using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class WebConfigProduct
    {
        public int ProductId { get; set; }
        public int WebConfigId { get; set; }
        public int? Index { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual WebConfig WebConfig { get; set; } = null!;
    }
}
