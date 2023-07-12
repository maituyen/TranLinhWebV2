using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyProject.Data;
using MyProject.Data.Entities;
using MyProject.Models;
using MyProject.ViewModels;
using System.ComponentModel.DataAnnotations; 

namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration; 
        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Authorize]
        [HttpGet]
        [Route("get")]
        public dynamic get()
        {
           var user= HttpContext.User.Claims;
            return Ok("OK");
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public dynamic Login(ViewModels.User.UserLoginVm user)
        {
            Models.Customer customer = new Models.Customer();
            customer = customer.GetByUser(user.Username);
            if (customer.Id == -1)
            {
                return BadRequest(new ApiBadRequestResponse<Models.Customer>("Tài khoản không tồn tại"));
            }
            if (customer.Password != GetMD5(user.Password))
            {
                return BadRequest(new ApiBadRequestResponse<Models.Customer>("Sai mật khẩu"));
            }
            string message = "Thành công";
            var tokenStr = GenrateJSONWebToken(customer); 
            return Ok(new { token = tokenStr, status = true, userId = customer.Id, fullname = customer.Fullname, message = message });
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("regis")]
        public dynamic Regis(Models.Customer user)
        {
            Models.Customer customer = new Models.Customer();
            customer = customer.GetByUser(user.Phone);
            if (user.Phone.Length != 10)
            {
                return BadRequest(new ApiBadRequestResponse<Models.Customer>("Số điện thoại không hợp lệ, số điện thoại phải có 10 số"));
            }
            if (user.Password.Length <6)
            {
                return BadRequest(new ApiBadRequestResponse<Models.Customer>("Mật khẩu không hợp lệ, mật khẩu phải có tối thiểu 6 ký tự"));
            }
            if (customer.Id != -1)
            {
                if(customer.Phone==user.Phone)
                    return BadRequest(new ApiBadRequestResponse<Models.Customer>("Số điện thoại đã được sử dụng")); 
            }
            customer = customer.GetByUser(user.Email);
            if (customer.Id != -1)
            {
                if (customer.Email == user.Email)
                    return BadRequest(new ApiBadRequestResponse<Models.Customer>("Email đã được sử dụng")); 
            }
            customer = new Models.Customer
            {
                Id = user.Id,
                Email = user.Email,
                Password = GetMD5(user.Password),
                Fullname = user.Fullname,
                Address = user.Address,
                Phone = user.Phone,
                IsClone = user.IsClone,
                CreatedAt = user.CreatedAt,
                Username = user.Username,
                Date = user.Date,
                Gender = user.Gender, 
            };
            customer = customer.Save();
            if (customer.Id == -1)
            {
                Helpers.Social.Buzz("Lỗi đăng ký tài khoản người dùng "+ Newtonsoft.Json.JsonConvert.SerializeObject(user));
                return BadRequest(new ApiBadRequestResponse<Models.Customer>("Xảy ra lỗi, bạn chưa thể đăng ký"));
            }
            else
            {
                return Ok(new ApiResponseServer<Models.Customer>(1, "Đăng ký tài khoản thành công"));
            }
        }
        private string GenrateJSONWebToken(Models.Customer user)
        {
            string key = _configuration["Jwt:Key"];  //URL nhan ket qua tra ve 
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            
            var cendentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {   new Claim(JwtRegisteredClaimNames.Name, user.Fullname), 
                new Claim(JwtRegisteredClaimNames.Email, user.Email), 
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: cendentials
            );
            var enCodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return enCodeToken;
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
        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
