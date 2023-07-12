using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyProject.Data;
using MyProject.Data.Entities;
using MyProject.Helpers;
using MyProject.ViewModels.User;

namespace MyProject.API
{
    [ApiController]
    public class LoginController : ApiBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;

        public LoginController(IConfiguration configuration, DatabaseContext context)
        {
            _configuration = configuration;
            _context = context;

        }
        #region google
        [AllowAnonymous]
        [Route("ggauth")]
        [HttpPost]
        public dynamic GGAuth()
        {
            return Ok(Models.LoginGGAuth.GetAutenticationURI());
        }
        #endregion
        [AllowAnonymous]
        [Route("auth")]
        [HttpPost]
        public dynamic Auth(UserLoginVm login)
        {

            var user = AuthenticateUser(login);
            var message = "";
            if (user != null)
            {
                if (user.IsBlock == true)
                {
                    message = "Tài khoản bạn bị khóa";
                    return new { token = "", status = false, message = message };
                }
                if (user.IsDelete == true)
                {
                    message = "Tài khoản bạn không tồn tại";
                    return new { token = "", status = false, message = message };
                }

                message = "Thành công";
                var tokenStr = GenrateJSONWebToken(user);
                return Ok(new { token = tokenStr, status = true, userId = user.Id, fullname = user.Fullname, message = message });
            } 
            return new { token = "", status = false, message = message };
        } 
        private User AuthenticateUser(UserLoginVm user)
        {
            var f_password = MD5Helper.GetMD5(user.Password);
            User userNull = null;
            var data = _context.Users.FirstOrDefault(m => m.Username == user.Username && m.Password == f_password);
            if (data != null)
            {
                return data;
            }
            return userNull;
        }

        private string GenrateJSONWebToken(User user)
        {
            string key = _configuration["Jwt:Key"];  //URL nhan ket qua tra ve 
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var cendentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMonths(120),
                signingCredentials: cendentials
            );
            var enCodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return enCodeToken;
        }

    }

}
