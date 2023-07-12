using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;

namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public ImageController(DatabaseContext context)
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
                var data = _context.Images.Include(m => m.Categor).AsQueryable();
                if (categoryId != 0)
                {
                    data = data.Where(m => m.Categor.Id == categoryId);
                }
                if (search != null || search.Length != 0)
                {
                    data = data.Where(m => m.Name.Contains(search));
                }
                return Ok(new
                {
                    Data = data.OrderByDescending(m => m.Categor.Id).Skip((page - 1) * size).Take(size).ToList(),
                    Count = data.Count()
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message, Status = false });
            }
        }
    }
}
