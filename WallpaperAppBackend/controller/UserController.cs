using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPatch("{uid}")]
        public async Task<ActionResult<User>> UpdateUserAsValues(int uid, JsonPatchDocument<User> patchDocument)
        {
            var existingUser = await context.userList.FirstOrDefaultAsync(User => User.UserId == uid);

            if (existingUser == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(existingUser);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await context.SaveChangesAsync();

            return Ok(existingUser);
        }

        [HttpPost("ResetPasswrod")]
        public async Task<ActionResult<string?>> ResetUserPassword(int uid, string newPassword)
        {
            var checkUserExits = await context.userList.FirstOrDefaultAsync(user=> user.UserId == uid);
            if (checkUserExits == null)
            {
                return NotFound(ModelState);
            }
            checkUserExits.Password = newPassword;
            await context.SaveChangesAsync();
            return Ok("password changed successfull");
        }
    }
}
