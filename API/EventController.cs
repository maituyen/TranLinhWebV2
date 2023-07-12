using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Data.Entities;
using Newtonsoft.Json;

namespace MyProject.API
{
    public class UploadEvent
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile? AdvertisementSmall { get; set; }
        public IFormFile? AdvertisementLarge { get; set; }
        public IFormFile? AdvertisementDetail { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Products { get; set; }
        public string ListCates { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public EventController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost("all")]
        public dynamic GetAll(dynamic obj)
        {
            int page = obj.page;
            int size = obj.size;
            var data = _context.Events.Include(m => m.Products).Include(m => m.Categories).Select(
                x => new
                {
                    x.Start,
                    x.End,
                    x.Name,
                    x.Id,
                    x.AdvertisementLarge,
                    x.AdvertisementDetail,
                    x.AdvertisementSmall,
                    x.Categories,
                    Products = from m in x.Products select new
                    {

                        Selected = true,
                        Link ="",
                             m.Id,
                             m.Name,
                             m.Description,
                             m.IsOld,
                             m.CreatedAt,
                             m.UpdatedAt,
                             m.IsPublish,
                             m.Sale,
                             m.Count,
                             m.UserId,
                             m.Promotion,
                             m.Vote,
                             m.KiotVietCode,
                             m.KiotVietName,
                             m.KiotVietPrice,
                             m.Prepay,
                             m.TheFirmId,
                             m.Capacity,
                             m.BundleOffer,
                             m.Blog,
                             m.Annotate,
                             m.Status,
                             m.EntryPrice,
                             m.SubsidyPrice,
                             m.CategoryId,
                             m.MasterProductId,
                             m.Tag,
                             m.IsProjectOld,
                             m.SeoUrl,
                             m.SeoName,
                             m.SeoDescription,
                             m.SeoSlug,
                             ProductDetails = from pd in m.ProductDetails
                                              select new
                                              {
                                                  pd.Id,
                                                  pd.ProductId,
                                                  pd.ProductCode,
                                                  pd.LinkImage,
                                                  pd.Name,
                                                  pd.Code,
                                                  pd.Color,
                                                  pd.Price
                                              },
                           
                    }
                });
            return Ok(new
            {
                Data = data.OrderBy(m => m.Start).Skip((page - 1) * size).Take(size).ToList(),
                Total = data.Count(),

            });
        }

        [HttpPost("update")]
        public dynamic UpdateEvent([FromForm] UploadEvent obj)
        {
            try
            {
                int Id = obj.Id;
                string Name = obj.Name;
                List<int> Products = JsonConvert.DeserializeObject<List<int>>(obj.Products);

                var inputEvent = new Event
                {
                    Id = Id,
                    Name = Name,
                    Start = obj.Start,
                    End = obj.End,

                };
                if (obj.AdvertisementSmall != null)
                {
                    string ImageNameAdvertisementSmall = Guid.NewGuid().ToString() + Path.GetExtension(obj.AdvertisementSmall.FileName);
                    string SavePathAdvertisementSmall = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs/upload", ImageNameAdvertisementSmall);
                    using (var stream = new FileStream(SavePathAdvertisementSmall, FileMode.Create))
                    {
                        obj.AdvertisementSmall.CopyTo(stream);
                    }
                    inputEvent.AdvertisementSmall = ImageNameAdvertisementSmall;
                }
                if (obj.AdvertisementLarge != null)
                {
                    string ImageNameAdvertisementLarge = Guid.NewGuid().ToString() + Path.GetExtension(obj.AdvertisementLarge.FileName);
                    string SavePathAdvertisementLarge = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs/upload", ImageNameAdvertisementLarge);
                    using (var stream = new FileStream(SavePathAdvertisementLarge, FileMode.Create))
                    {
                        obj.AdvertisementLarge.CopyTo(stream);
                    }
                    inputEvent.AdvertisementLarge = ImageNameAdvertisementLarge;
                }
                if (obj.AdvertisementDetail != null)
                {
                    string ImageNameAdvertisementDetail = Guid.NewGuid().ToString() + Path.GetExtension(obj.AdvertisementDetail.FileName);
                    string SavePathAdvertisementDetail = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs/upload", ImageNameAdvertisementDetail);
                    using (var stream = new FileStream(SavePathAdvertisementDetail, FileMode.Create))
                    {
                        obj.AdvertisementDetail.CopyTo(stream);
                    }
                    inputEvent.AdvertisementDetail = ImageNameAdvertisementDetail;
                }
                var item = _context.Events.Include(m => m.Products).Include(m => m.Categories).FirstOrDefault(m => m.Id == Id);
                if (item == null)
                {

                    _context.Events.Add(inputEvent);
                    _context.SaveChanges();

                    foreach (var perId in Products)
                    {
                        Product product = new Product { Id = perId };
                        _context.Products.Add(product);
                        _context.Products.Attach(product);
                        product.Events.Add(inputEvent);
                        _context.SaveChanges();
                    }
                    return true;
                }
                item.Name = inputEvent.Name;
                item.Start = inputEvent.Start;
                item.End = inputEvent.End;
                if (obj.AdvertisementSmall != null)
                {
                    item.AdvertisementSmall = inputEvent.AdvertisementSmall;
                }
                if (obj.AdvertisementLarge != null)
                {
                    item.AdvertisementLarge = inputEvent.AdvertisementLarge;
                }
                if (obj.AdvertisementDetail != null)
                {
                    item.AdvertisementDetail = inputEvent.AdvertisementDetail;
                }

                //remove productevent
                foreach (var product in item.Products.ToList())
                {
                    if (!Products.Contains(product.Id))
                        item.Products.Remove(product);
                }
                // add product
                var newProducts = _context.Products.Where(r => Products.Contains(r.Id))
                  .ToList();
                item.Products.Clear();
                foreach (var newProduct in newProducts)
                {
                    item.Products.Add(newProduct);
                }

                List<int> ListCates = JsonConvert.DeserializeObject<List<int>>(obj.ListCates);

                //remove categoryEvent
                foreach (var category in item.Categories.ToList())
                {
                    if (!ListCates.Contains(category.Id))
                        item.Categories.Remove(category);
                }
                // add category
                var newCategories = _context.Categories.Where(r => ListCates.Contains(r.Id))
                  .ToList();
                item.Categories.Clear();
                foreach (var newCategory in newCategories)
                {
                    item.Categories.Add(newCategory);
                }
                _context.SaveChanges();
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        [HttpPost("delete/{id}")]
        public dynamic RemoveEvent(int id)
        {

            var item = _context.Events.FirstOrDefault(m => m.Id == id);
            _context.Events.Remove(item);
            _context.SaveChanges();
            return _context.Events.ToList();
        }
    }
}
