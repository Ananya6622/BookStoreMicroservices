using BookStore.User.Entity;
using BookStore.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookStore.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser user;
        public UserController(IUser user)
        {
            this.user = user;
        }

        [HttpPost("Register")]
        public IActionResult UserRegister(UserEntity userEntity)
        {
            try
            {
                var result = user.UserRegistration(userEntity);
                if (result != null)
                {
                    return this.Ok(new ResponseModel<UserEntity> { Status = true, Message = "User Registration successfull", Data = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "user Registration unsuccessufull" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("Login")]
        public IActionResult USerLogin(string email, string password)
        {
            try
            {
                var result = user.UserLogin(email, password);
                if (result != null)
                {
                    return this.Ok(new ResponseModel<string> { Status = true, Message = "user login successfull", Data = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "user login unsuccessufull" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("MyProfile")]
        public IActionResult MyProfile(int userId)
        {
            try
            {
                long UserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = user.MyProfile(userId);
                if(result != null)
                {
                    return this.Ok(new ResponseModel<UserEntity> { Status = true, Message = "fetched data", Data = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "unable to fetch data" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
            
        }
    }
}
