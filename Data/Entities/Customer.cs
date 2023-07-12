using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Comments = new HashSet<Comment>();
            CustomerHistories = new HashSet<CustomerHistory>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Fullname { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public bool? IsClone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Username { get; set; }
        public DateTime? Date { get; set; }
        public bool? Gender { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<CustomerHistory> CustomerHistories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
