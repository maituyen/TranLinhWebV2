using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Data.Entities;

namespace MyProject.API

{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public PermissionController(DatabaseContext context)
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
                var data = _context.Permissions.Select(m => new
                {
                    m.Id,
                    m.Name,
                    ListRoles = from role in m.Roles
                                select new
                                {
                                    role.Id,
                                    role.ActionCode,
                                    role.ActionName
                                }

                });
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
                string Name = obj["Name"];
                List<int> ListRoles = obj["ListRoles"].ToObject<List<int>>();

                var per = new Permission();
                if (per == null)
                {
                    return BadRequest(new { message = "Không tồn tại", status = false });
                }

                per.Name = Name;
                _context.Permissions.Add(per);
                foreach (var roleId in ListRoles)
                {
                    Role role = new Role { Id = roleId };
                    _context.Roles.Add(role);
                    _context.Roles.Attach(role);
                    role.Permistions.Add(per);
                    _context.SaveChanges();

                }

                return Ok(new { message = "Thành công", status = true });
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
                string Name = obj["Name"];
                List<int> ListRoles = obj["ListRoles"].ToObject<List<int>>();

                var per = _context.Permissions.FirstOrDefault(m => m.Id == id);
                if (per == null)
                {
                    return BadRequest(new { message = "Không tồn tại", status = false });
                }
                per.Name = Name;

                foreach (var roleId in ListRoles)
                {
                    Role role = new Role { Id = roleId };
                    _context.Roles.Add(role);
                    _context.Roles.Attach(role);
                    role.Permistions.Add(per);
                    _context.SaveChanges();

                }

                return Ok(new { message = "Thành công", status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("remove/permistionrole/{perId}")]
        public dynamic RemovePermistionrole(int perId)
        {
            try
            {
                var per = _context.Permissions.Include(z => z.Roles).FirstOrDefault(m => m.Id == perId); ;
                foreach (Role c in per.Roles.ToList())
                {
                    per.Roles.Remove(c);
                }
                _context.SaveChanges();

                return Ok(new { message = "Thành công", status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }


        [Authorize]
        [HttpPost]
        [Route("remove/{perId}")]
        public dynamic Remove(int perId)
        {
            try
            {
                var per = _context.Permissions.FirstOrDefault(m => m.Id == perId); ;
                _context.Permissions.Remove(per);
                _context.SaveChanges();

                return Ok(new { message = "Thành công", status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }
    }
}
