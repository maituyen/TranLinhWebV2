using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class Metadatum
    {
        public Metadatum()
        {
            ProductMetadata = new HashSet<ProductMetadatum>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Sort { get; set; }

        public virtual ICollection<ProductMetadatum> ProductMetadata { get; set; }
    }
}
