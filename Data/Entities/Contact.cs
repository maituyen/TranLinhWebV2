using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string? Action { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsRead { get; set; }
        public string? Title { get; set; }
        public int? ProductId { get; set; }
        public int? CustomerId { get; set; }
        public string? ProductDetails { get; set; }
        public int? FromProjectId { get; set; }
        public int? ToProjectId { get; set; }
        public int? ProductTradeInsStatus { get; set; }
        public string? TradeInType { get; set; }
        public string? BankInstallment { get; set; }
        public string? RatioInstallment { get; set; }
        public string? MonthInstallment { get; set; }
        public string? Address { get; set; }
        public string? Cmnd { get; set; }
        public string? Birthday { get; set; }
        public string? InterestRate { get; set; }
    }
}
