using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class User
    {
        public User()
        {
            Blogs = new HashSet<Blog>();
            Products = new HashSet<Product>();
            Permissions = new HashSet<Permission>();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool? IsBlock { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsAdmin { get; set; }
        public string? Fullname { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
