using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Data.Entities;


namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public UserController(DatabaseContext context)
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
                var data = _context.Users.Where(m => m.IsDelete == false && m.IsBlock == false).Include(m => m.Permissions).ToList();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public dynamic GetById(int id)
        {
            try
            {
                var item = _context.Users.Include(m => m.Permissions)
                    .Select(m => new
                    {
                        m.Id,
                        m.Username,
                        m.IsAdmin,
                        Permissions = from per in m.Permissions
                                      select new
                                      {
                                          per.Id,
                                          per.Name,
                                          Roles = from r in per.Roles
                                                  select new
                                                  {
                                                      r.Id,
                                                      r.ActionName,
                                                      r.ActionCode
                                                  }
                                      }
                    })
                    .FirstOrDefault(_ => _.Id == id);
                return Ok(item);
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
                string email = obj["Email"];
                string username = obj["Username"];
                string fullname = obj["Fullname"];
                string password = obj["Password"];
                List<int> ListPermistions = obj["ListPermistions"].ToObject<List<int>>();
                var user = new User
                {
                    Email = email,
                    Username = username,
                    Password = GetMD5(password),
                    Fullname = fullname,
                    IsBlock = false,
                    IsDelete = false,
                    IsAdmin = false
                };
                var checkUser = _context.Users.Where(m => m.Username == user.Username).ToList();
                if (checkUser.Count() > 0)
                {
                    return BadRequest(new { message = "Đã tồn tại tên đăng nhập, Nhập lại tên đăng nhập", status = false });
                }
                _context.Users.Add(user);
                _context.SaveChanges();

                foreach (var perId in ListPermistions)
                {
                    Permission permission = new Permission { Id = perId };
                    _context.Permissions.Add(permission);
                    _context.Permissions.Attach(permission);
                    permission.Users.Add(user);
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
        public dynamic Update(int id, dynamic obj)
        {
            try
            {
                string email = obj["Email"];
                string fullname = obj["Fullname"];
                string password = obj["Password"];
                List<int> ListPermistions = obj["ListPermistions"].ToObject<List<int>>();
                var checkUser = _context.Users.Find(id);
                if (checkUser != null)
                {
                    checkUser.Fullname = fullname;
                    checkUser.Email = email;
                    //    checkUser.Password = password;
                    _context.SaveChanges();

                    foreach (var perId in ListPermistions)
                    {
                        Permission permission = new Permission { Id = perId };
                        _context.Permissions.Add(permission);
                        _context.Permissions.Attach(permission);
                        permission.Users.Add(checkUser);
                        _context.SaveChanges();
                    }

                    return Ok(new { message = "Thành công", status = true });
                }
                else
                {
                    return BadRequest(new { message = "Không tồn tại tài khoản", status = false });

                }
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }


        [Authorize]
        [HttpPost]
        [Route("password/{id}")]
        public dynamic UpdatePassword(int id, dynamic obj)
        {
            try
            {
                string password = obj["Password"];
                var checkUser = _context.Users.Find(id);
                if (checkUser != null)
                {
                    checkUser.Password = GetMD5(password);
                    _context.SaveChanges();
                    return Ok(new { message = "Thành công", status = true });
                }
                else
                {
                    return BadRequest(new { message = "Không tồn tại tài khoản", status = false });

                }
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }


        [Authorize]
        [HttpGet]
        [Route("remove/permistionuser/{userId}")]
        public dynamic RemovePermistionUser(int userId)
        {
            try
            {
                var user = _context.Users.Include(z => z.Permissions).FirstOrDefault(m => m.Id == userId); ;
                foreach (Permission c in user.Permissions.ToList())
                {
                    user.Permissions.Remove(c);
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
        [Route("delete/{id}")]
        public dynamic Delete(int id)
        {
            try
            {
                var checkUser = _context.Users.Find(id);
                if (checkUser != null)
                {
                    if (checkUser.IsAdmin == false)
                    {
                        _context.Users.Remove(checkUser);
                        _context.SaveChanges();
                        return Ok(new { message = "Thành công", status = true });
                    }
                    else
                    {
                        return Ok(new { message = "Bạn không thể xóa admin hệ thống", status = false });

                    }
                }
                else
                {
                    return Ok(new { message = "Không tồn tại tài khoản", status = false });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }

        private string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

    }
}
