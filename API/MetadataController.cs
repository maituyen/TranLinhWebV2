using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Data.Entities;

namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetadataController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public MetadataController(DatabaseContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("all")]
        public dynamic GetAll()
        {
            try
            {
                var data = _context.Metadata.Include(m => m.ProductMetadata).ToList();
                return Ok(new { Message = "Thành công", Data = data, Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message, Status = false });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("product/{id}")]
        public dynamic GetByProductId(int id)
        {
            try
            {
                var data = _context.ProductMetadata.Include(m => m.Metadata)
                    .Where(pm => pm.ProductId == id)
                    .ToList();

                return Ok(new { Message = "Thành công", Data = data, Status = true });
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
                var metadatum = new Metadatum
                {
                    Name = obj.Name,
                    Sort = (int)obj.Sort,
                };
                _context.Metadata.Add(metadatum);
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
        [Route("update/{id}")]
        public dynamic Update(dynamic metadatum, int id)
        {
            try
            {
                var item = _context.Metadata.Find(id);
                if (item == null)
                {
                    return BadRequest(new { Message = "Không tồn tại", Status = false });
                }
                item.Sort = (int)metadatum.Sort;
                item.Name = metadatum.Name;
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
                var item = _context.Metadata.Find(id);
                if (item == null)
                {
                    return BadRequest(new { Message = "Không tồn tại", Status = false });
                }
                _context.Metadata.Remove(item);
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
