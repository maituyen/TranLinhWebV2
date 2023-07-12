using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Data.Entities;


namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public ContentController(DatabaseContext context)
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
                var data = _context.Contents.Include(m => m.Categories).AsQueryable();
                if (categoryId != 0)
                {
                    data = data.Where(m => m.Categories.Any(cate => cate.Id == categoryId));
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
        public dynamic Add(dynamic obj)
        {
            try
            {
                List<int> listIds = obj["CategoryIds"].ToObject<List<int>>();
                string name = obj.Name;
                string description = obj.Description;
                var content = new Content
                {
                    Name = name,
                    Description = description
                };
                _context.Contents.Add(content);
                foreach (var listId in listIds)
                {
                    Category category = new Category { Id = listId };
                    _context.Categories.Add(category);
                    _context.Categories.Attach(category);
                    category.Contents.Add(content);
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
        public dynamic Update(dynamic obj)
        {
            try
            {
                List<int> listIds = obj["CategoryIds"].ToObject<List<int>>();
                string name = obj.Name;
                string description = obj.Description;
                int id = obj.Id;
                var item = _context.Contents.Include(m => m.Categories).FirstOrDefault(m => m.Id == id);
                if (item == null)
                {
                    return BadRequest(new { Message = "Không tồn tại", Status = false });

                }
                item.Name = name;
                item.Description = description;

                foreach (var p in item.Categories.ToList())
                {
                    if (!listIds.Contains(p.Id))
                        item.Categories.Remove(p);
                }
                var newItems = _context.Categories.Where(r => listIds.Contains(r.Id))
                  .ToList();
                item.Categories.Clear();

                foreach (var newItem in newItems)
                {
                    item.Categories.Add(newItem);
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
        [Route("delete/{id}")]
        public dynamic Delete(int id)
        {
            try
            {
                var item = _context.Contents.Find(id);
                if (item == null)
                {
                    return BadRequest(new { Message = "Không tồn tại", Status = false });
                }
                _context.Contents.Remove(item);
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
