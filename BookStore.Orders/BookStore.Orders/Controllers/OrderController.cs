using BookStore.Orders.Entity;
using BookStore.Orders.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using BookStore.Orders.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace BookStore.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder order;
        
        public OrderController(IOrder order)
        {
            this.order = order;
            
        }
        
        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder(int bookId, int quantity)
        {
            try
            {
                string s = HttpContext.Request.Headers["Authorization"].ToString();
                s = s.Substring("bearer".Length);
                var result =await order.AddOrder(bookId,quantity,s);
                if (result != null)
                {
                    return this.Ok(new ResponseModel<OrderEntity> { Status = true, Message = "added order successfully", Data = result });
                }
                else
                {
                    return this.BadRequest(new { Status = false, message = "unable to add order" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            try
            {
                var result = order.GetAllOrders();
                if (result != null)
                {
                    return this.Ok(new ResponseModel<List<OrderEntity>> { Status = true, Message = "retrieved all orders", Data = result });
                }
                else
                {
                    return this.BadRequest(new { Status = false, message = "unable to get orders" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        //[HttpGet("GetBookDetails/{bookId}")]
        //public async Task<IActionResult> GetBookDetails(int bookId)
        //{
        //    try
        //    {
        //        BookEntity book = await appCalling.GetBookDetailsById(bookId);

        //        if (book != null)
        //        {
        //            return Ok(new ResponseModel<BookEntity> { Status = true, Message = "Book details retrieved successfully", Data = book });
        //        }
        //        else
        //        {
        //            return NotFound(new ResponseModel<BookEntity> { Status = false, Message = "Book not found", Data = null });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ResponseModel<BookEntity> { Status = false, Message = "Internal server error", Data = null });
        //    }
        //}
        
        //[HttpGet("GetUserDetails/{userId}")]
        //public async Task<IActionResult> GetUserDetails(string token)
        //{
        //    try
        //    {
        //        UserEntity user = await appCalling.GetUserDetails(token);

        //        if (user != null)
        //        {
        //            return Ok(new ResponseModel<UserEntity> { Status = true, Message = "user details retrieved successfully", Data = user });
        //        }
        //        else
        //        {
        //            return NotFound(new ResponseModel<BookEntity> { Status = false, Message = "user not found", Data = null });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ResponseModel<BookEntity> { Status = false, Message = "Internal server error", Data = null });
        //    }
        //}
    }
}
