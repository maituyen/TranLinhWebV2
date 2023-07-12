using System;
using System.Collections.Generic;

namespace MyProject.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? Status { get; set; }
        public string? SenderName { get; set; }
        public string? SenderTelephone { get; set; }
        public string? SenderEmail { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Wards { get; set; }
        public string? SenderAddress { get; set; }
        public string? SenderNote { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? SaleTotal { get; set; }
        public bool? IsUpdateExcel { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
