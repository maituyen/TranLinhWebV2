using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class ProductMetadatum
    {
        public int ProductId { get; set; }
        public int MetadataId { get; set; }
        public string? Value { get; set; }

        public virtual Metadatum Metadata { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
