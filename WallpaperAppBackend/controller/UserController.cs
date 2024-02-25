using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WallpaperAppBackend.Context;
using WallpaperAppBackend.Model;
using WallpaperAppBackend.Services;

namespace WallpaperAppBackend.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly WallpaperDBcontext context;
        public UserController(WallpaperDBcontext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
           return await context.userList.ToListAsync();
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> RegisterUser(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            var checkUser = await context.userList.FirstOrDefaultAsync(usr => usr.Email == user.Email);
            if (checkUser != null)
            {
                return Conflict("username already exits");
            }
         
            context.userList.Add(user);
            await context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser(string email, string password)
        {
            var checkUserIsValid = await context.userList.FirstOrDefaultAsync(user => user.Email == email && user.Password == password);
            if (checkUserIsValid == null)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<string?>> ForgotPasswordUser(string? email)
        {
            var checkEmailExits = await context.userList.FirstOrDefaultAsync(user => user.Email == email);
            if(checkEmailExits == null)
            {
                return NotFound();
            }
            EmailService.SendForgotPasswordMail(checkEmailExits);
            return Ok("Email sent");
        }
    }
}
