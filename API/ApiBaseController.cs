using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyProject.API; 
[ApiController]
[Route("api/v1/[controller]")]
//[Route("api/v1/admin/[controller]")]
public class ApiBaseController: ControllerBase
{

}
