using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Data;
using MyProject.Data.Entities;

namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public RoleController(DatabaseContext context)
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
                var data = _context.Roles.ToList();
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
        public dynamic Add(dynamic obj)
        {
            try
            {
                string ActionName = obj["ActionName"];
                string ActionCode = obj["ActionCode"];
                var role = new Role
                {
                    ActionName = ActionName,
                    ActionCode = ActionCode,

                };
                _context.Roles.Add(role);
                _context.SaveChanges();
                return Ok(new { message = "Thành công", role = role, status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("update/{id}")]
        public dynamic Update(dynamic obj, int id)
        {
            try
            {

                string ActionName = obj["ActionName"];
                string ActionCode = obj["ActionCode"];
                var check = _context.Roles.Find(id);
                if (check != null)
                {
                    check.ActionName = ActionName;
                    check.ActionCode = ActionCode;
                    _context.SaveChanges();
                    return Ok(new { message = "Thành công", status = true });
                }

                return BadRequest(new { message = "Không tồn tại", status = false });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("delete/{id}")]
        public dynamic Delete(int id)
        {
            try
            {
                var check = _context.Roles.Find(id);
                if (check != null)
                {
                    _context.Roles.Remove(check);
                    _context.SaveChanges();
                    return Ok(new { message = "Thành công", status = true });
                }
                else
                {
                    return BadRequest(new { message = "Không tồn tại", status = false });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }
    }
}
