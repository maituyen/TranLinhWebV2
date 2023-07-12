using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Data.Entities;

namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public CommentController(DatabaseContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("pageNumber/{pageNumber}/pageSize/{pageSize}")]
        public dynamic GetAll(int pageSize, int pageNumber)
        {
            try
            {
                var data = _context.Comments.Include(m => m.Products).Where(m => m.ParentId== null );

                return Ok(new
                {
                    data = data.OrderBy(m => m.CreatedAt).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                    count = data.Count()
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("count")]
        public dynamic GetTotalCommentChecking()
        {
            try
            {
                var data = _context.Comments.Where(m => m.IsShow == false).Count();

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("add")]
        public dynamic AddNew(dynamic obj)
        {
            try
            {
                List<int> ListProduct = obj["ListProduct"].ToObject<List<int>>();
                var comment = new Comment
                {
                    CreatedAt = DateTime.Now,
                    Vote = obj["Vote"],
                    Description = obj["Description"],
                    IsShow = (bool)obj["IsShow"],
                   
                };
                if (obj["ParentId"] != null)
                {
                    comment.ParentId = (int)obj["ParentId"];
                }

                _context.Comments.Add(comment);
                _context.SaveChanges();
                foreach (var Id in ListProduct)
                {
                    Product product = new Product { Id = Id };
                    _context.Products.Add(product);
                    _context.Products.Attach(product);
                    product.Comments.Add(comment);
                    _context.SaveChanges();

                }
                return Ok(new { message = "Thành công", status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("update/{id}")]
        public dynamic Update(dynamic obj, int id)
        {
            try
            {
                var check = _context.Comments.FirstOrDefault(m => m.Id == id);
                if (check == null)
                {
                    return BadRequest(new { message = "Không tồn tại", status = true });
                }
                check.Vote = (int)obj["Vote"];
                check.Description = obj["Description"];
                check.IsShow = (bool)obj["IsShow"];
                _context.SaveChanges();

                return Ok(new { message = "Thành công", status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("remove/{id}")]
        public dynamic Remove(int id)
        {
            try
            {
                var comment = _context.Comments.Find(id);
                if(comment ==null)
                {
                    return BadRequest("faild");
                }
                _context.Comments.Remove(comment);
                _context.SaveChanges();

                return Ok(new { message = "Thành công", status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("find/child/{parentid}")]
        public dynamic FindChild(int parentid)
        {
            try
            {
                var comment = _context.Comments.Where(m => m.ParentId == parentid);


                return Ok(new { message = "Thành công", status = true, Data = comment });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("toggle")]
        public dynamic Toggle(dynamic obj)
        {
            try
            {
                var item = _context.Comments.Find((int)obj.Id);
                if (item == null)
                {
                    return BadRequest(new { Message = "Không tồn tại", Status = false });
                }
                item.IsShow = (bool)obj.IsShow;
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
