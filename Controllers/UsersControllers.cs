using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SimajaAPI;
using SimajaAPI.EntitySimaja;

namespace MyApp.Namespace
{
    [Route("api/users")]
    [ApiController]

    [Authorize]
    public class UsersControllers : ControllerBase
    {
        readonly JwtTokenHelper jwtHelper;
        readonly SimajaDbContext dbContext;

        public UsersControllers(JwtTokenHelper _jwtHelper , SimajaDbContext _context)
        {
            jwtHelper = _jwtHelper;
            dbContext = _context;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginForm _login)
        {
            try
            {
                if (_login == null) return BadRequest("Missing Parameter email or password"); 
                if (dbContext.Login(_login.username! , _login.password! , out var user))
                {
                    var guid = Guid.NewGuid().ToString();
                    var token = jwtHelper.GenerateToken(guid , user.username! , user.role.ToString());
                    var response = new ResponseLogin
                    {
                        userId = guid,
                        username = user.username,
                        role = user.roles!.roleName ,
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

        [HttpGet("profile")]
        [Authorize]
        [EnableRateLimiting("fixed")]
        public IActionResult GetProfile()
        {
            var token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            try
            {
                var payload = jwtHelper.DecodeToken(token);
                return Ok(payload);
            }catch (Exception e)
            {
                return StatusCode(407, e.Message);
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
        public string? userId { get; set; }
        public string? username { get; set; }
        public string? role { get; set; }
        public string? access_token { get; set; }
    }
}
