using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace MyProject.API;
[Route("api/[controller]")]
[ApiController] 
public class AccountController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService; 
    public AccountController(IAuthenticationService authenticationService )
    {
        _authenticationService = authenticationService; 
    }
    [HttpGet]
    [Route("fb")]
    public dynamic LoginWithFacebook()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = "/gio-hang" // Địa chỉ URL chuyển hướng sau khi đăng nhập thành công
        }; 
        return Ok(Challenge(properties, "Facebook"));
    }
    public async Task<IActionResult> ExternalLoginCallback()
    {
        var result = await HttpContext.AuthenticateAsync();

        if (result.Succeeded)
        {
            // Xử lý thông tin xác thực của người dùng, ví dụ:
            var userId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = result.Principal.FindFirstValue(ClaimTypes.Name);

            // Thực hiện các thao tác khác sau khi đăng nhập thành công
        }
        else
        {
            // Xử lý khi đăng nhập không thành công
        }

        return RedirectToAction("Index", "Home");
    }

}