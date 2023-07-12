using System;
using System.Collections.Generic;

namespace MyProject.Data.Entities
{
    public partial class Event
    {
        public Event()
        {
            Categories = new HashSet<Category>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string? AdvertisementSmall { get; set; }
        public string? AdvertisementLarge { get; set; }
        public string? AdvertisementDetail { get; set; }
        public DateTime? Start { get; set; }
        public string? Name { get; set; }
        public DateTime? End { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
