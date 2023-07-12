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
    [Route("api/[controller]")]
    public class SiteInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration; 
        public SiteInfoController(IConfiguration configuration)
        {
            _configuration = configuration; 
        } 
        [HttpGet("info")]
        [AllowAnonymous]
        public dynamic Info()
        {
            try
            {
                Data.Entities.SiteInfo siteInfo = new Data.Entities.SiteInfo(Request.Host.Host);
                return Ok(siteInfo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("list")]
        [AllowAnonymous]
        public dynamic GetAll()
        {
            try
            {
                Data.Entities.SiteInfo siteInfo = new Data.Entities.SiteInfo();
                return Ok(siteInfo.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }

}
