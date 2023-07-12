using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class Role
    {
        public Role()
        {
            Permistions = new HashSet<Permission>();
        }

        public int Id { get; set; }
        public string? ActionName { get; set; }
        public string? ActionCode { get; set; }

        public virtual ICollection<Permission> Permistions { get; set; }
    }
}
