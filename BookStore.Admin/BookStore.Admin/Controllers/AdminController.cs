using BookStore.Admin.Entity;
using BookStore.Admin.Entity.QueryEntity;
using BookStore.Admin.Interfaces;
using BookStore.Admin.Services;
using BookStore.Books.Entity;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookStore.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin admin;
        //private readonly ApiCalling apiCalling;
        public AdminController(IAdmin admin)
        {
            this.admin = admin;
            //this.apiCalling = apiCalling;
        }

        [HttpPost("Register")]
        public IActionResult AdminRegister(AdminEntity adminEntity)
        {
            try
            {
                var result = admin.AdminRegistration(adminEntity);
                if(result != null)
                {
                    return this.Ok(new ResponseModel<AdminEntity> { Status = true, Message = "Admin Registration successfull", Data = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Admin Registration unsuccessufull" });
                }
            }
            catch(Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("Login")]
        public IActionResult AdminLogin(string email, string password)
        {
            try
            {
                var result = admin.AdminLogin(email,password);
                if(result != null)
                {
                    return this.Ok(new ResponseModel<AdminEntity> { Status = true, Message = "Admin login successfull", Data = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Admin login unsuccessufull" });
                }
            }
            catch(Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("apiBooks")]
        public async Task<IActionResult> ApiBooks()
        {
            try
            {
                ApiCalling apiCalling = new ApiCalling();
                var result = await apiCalling.GetOutOfStockBooks();
                if(result != null)
                {
                    return this.Ok(new ResponseModel<ViewBook> { Status = true, Message = "retrieved out of stock books", Data = result });
                }
                else
                {
                    return this.BadRequest(new { status = false, message = "unable to retrieve" });
                }
            }
            catch(Exception ex)
            {
                return this.BadRequest(new {success  = false, message = ex.Message});
            }
        }
    }
}
