using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Data.Entities;

namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyWordController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public KeyWordController(DatabaseContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        [Route("data")]
        public dynamic GetAll(dynamic obj)
        {
            try
            {
                int page = obj.Page;
                int size = obj.Size;
                int categoryId = obj.CategoryId;
                string search = obj.Search;
                var data = _context.KeyWords.Include(m => m.Category).AsQueryable();
                if (categoryId != 0)
                {
                    data = data.Where(m => m.CategoryId == categoryId);
                }
                if (search != null || search.Length != 0)
                {
                    data = data.Where(m => m.Name.Contains(search));
                }
                return Ok(new
                {
                    Data = data.OrderByDescending(m => m.Id).Skip((page - 1) * size).Take(size).ToList(),
                    Count = data.Count()
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message, Status = false });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public dynamic Add(dynamic input)
        {
            try
            {
                List<dynamic> lists = input["List"].ToObject<List<dynamic>>();
                foreach (var obj in lists)
                {
                    int KiotVietPrice = obj.KiotVietPrice;
                    string KiotVietCode = obj.KiotVietCode;
                    string KiotVietName = obj.KiotVietName;
                    var product = new Product
                    {
                        KiotVietName = KiotVietName,
                        KiotVietCode = KiotVietCode,
                        KiotVietPrice = KiotVietPrice,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Blog = obj.Blog,

                    };
                    _context.Products.Add(product);
                }

                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message, Status = false });
            }
        }
        [Authorize]
        [HttpPost]
        [Route("remove/{id}")]
        public dynamic RemoveKeyword(int id)
        {
            try
            {
                var item = _context.KeyWords.Find(id);
                if(item != null)
                {
                  _context.KeyWords.Remove(item);   
                }
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message, Status = false });
            }
        }


        [Authorize]
        [HttpPost]
        [Route("update")]
        public dynamic AddKeyword([FromForm]UploadKeyword  obj)
        {
            try
            {
                var inputEvent = new KeyWord
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Description =obj.Description,
                    CategoryId = obj.CategoryId,
                    Blog = obj.Blog,
                    Slug = SlugGenerator.SlugGenerator.GenerateSlug(obj.Name.ToLower())
                };
                string imageName = "";
                if (obj.File != null)
                {
                     imageName = Guid.NewGuid().ToString() + Path.GetExtension(obj.File.FileName);
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs/upload", imageName);
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        obj.File.CopyTo(stream);
                    }
                    inputEvent.LinkImage = imageName; 
                }
                var item = _context.KeyWords.Find(obj.Id);
                if(item == null)
                {
                    _context.KeyWords.Add(inputEvent);
                    _context.SaveChanges();
                    return Ok(new { Message = "Thành công", Status = true });
                }
                item.Name = inputEvent.Name;
                item.CategoryId= inputEvent.CategoryId;
                item.Description= inputEvent.Description;
                item.Slug = inputEvent.Slug;
                item.Blog=inputEvent.Blog;
                if (obj.File != null)
                {
                    item.LinkImage = imageName;
                }
               _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message, Status = false });
            }
        }

    }
}

public class UploadKeyword
{

    public int Id { get; set; }
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public string? Description { get; set; }
    public string? Blog { get; set; }
    public IFormFile? File { get; set; }
}
 
