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
    public class AdminController : ControllerBase
    { 
        public AdminController()
        {
        } 
        [Authorize]
        [HttpGet]
        [Route("menu")]
        public dynamic GetMenu()
        {
            try
            {
                Models.Admin.Menu menu = new Models.Admin.Menu(); 
                return Ok(menu.GetParent(0));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
         
    }
}
