using Microsoft.AspNetCore.Mvc;
namespace UsersController.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    [TrimStrings]
    public IActionResult CreateUser([FromBody] User user)
    {
       return Ok(user);
    }
}