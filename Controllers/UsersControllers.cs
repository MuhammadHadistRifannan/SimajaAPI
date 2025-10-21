using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SimajaAPI; 

namespace MyApp.Namespace
{
    [Route("api/users")]
    [ApiController]
    public class UsersControllers : ControllerBase
    {
        readonly JwtTokenHelper jwtHelper;

        public UsersControllers(JwtTokenHelper _jwtHelper)
        {
            jwtHelper = _jwtHelper;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginForm _login)
        {
            try
            {
                 if (_login == null) return BadRequest("Missing Parameter email or password"); 
                if (_login.username == "hadist" && _login.password == "admin123")
                {
                    var token = jwtHelper.GenerateToken(_login.username);
                    var response = new ResponseLogin
                    {
                        username = _login.username ,
                        access_token = token
                    };

                    return Ok(response);
                }
                return Unauthorized("Wrong Password or Username");

            }catch (Exception e)
            {
                return Unauthorized();
            }
        }

        [HttpGet("profile/{id}")]
        [Authorize]
        public IActionResult GetProfile(int id)
        {
            try
            {
                if (id == 1)
                {
                    return Ok("users hadist found ");
                }else
                {
                    return NotFound("Users id 1 not found");
                }
            }
            catch (Exception e)
            {
                return Unauthorized("You must be Authorized first");
            }
        }
    }

    public class LoginForm
    {
        public string? username { get; set; }
        public string? password { get; set; }
    }

    public class ResponseLogin
    {
        public string? username { get; set; }
        public string? access_token { get; set; }
    }
}
