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
using System.Security.Claims;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                s = s.Substring("Bearer ".Length);
                UserEntity user = await AppCalling.GetUserDetails(s);
                var result = await order.AddOrder(bookId, quantity, s);

                if (result != null)
                {
                    string Key = "3bJt9f";
                    string Salt = "fgDHdaLqmMhuhewf3md1k7nqBfx7B4bi";

                    // Generate transaction ID
                    string transactionId = order.GenerateTransactionID();

                    // Calculate hash
                    string hash = order.GenerateHash(new PayURequest
                    {
                        TransactionId = transactionId,
                        Amount = result.OrderAmount,
                        ProductInfo = "BookDetails", 
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.MobileNumber,
                        Surl = "https://localhost:44338/api/Order/GetSuccessOrders",
                        Furl = "https://localhost:44338/api/Order/GetFailOrders",
                    }, Key, Salt, transactionId);

                    // Declare paymentRequest after hash calculation
                    var paymentRequest = new PayURequest
                    {
                        Key = Key,
                        TransactionId = transactionId,
                        Amount = result.OrderAmount,
                        ProductInfo = "BookDetails",
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.MobileNumber,
                        Surl = "https://localhost:44338/api/Order/GetSuccessOrders",
                        Furl = "https://localhost:44338/api/Order/GetFailOrders",
                        Hash = hash
                    };

                    string paymentResponse = await order.SendPaymentRequestAsync(paymentRequest);

                    return this.Ok(new ResponseModel<string> { Status = true, Message = "added order successfully", Data = paymentResponse });
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
        [HttpGet("GetBookDetails/{bookId}")]
        public async Task<IActionResult> GetBookDetails(int bookId)
        {
            try
            {
                BookEntity book = await AppCalling.GetBookDetailsById(bookId);

                if (book != null)
                {
                    return Ok(new ResponseModel<BookEntity> { Status = true, Message = "Book details retrieved successfully", Data = book });
                }
                else
                {
                    return NotFound(new ResponseModel<BookEntity> { Status = false, Message = "Book not found", Data = null });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<BookEntity> { Status = false, Message = "Internal server error", Data = null });
            }
        }

        [HttpGet("GetUserDetails/{userId}")]
        public async Task<IActionResult> GetUserDetails()
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString();
                token = token.Substring("Bearer ".Length);


                UserEntity user = await AppCalling.GetUserDetails(token);

                if (user != null)
                {
                    return Ok(new ResponseModel<UserEntity> { Status = true, Message = "user details retrieved successfully", Data = user });
                }
                else
                {
                    return NotFound(new ResponseModel<BookEntity> { Status = false, Message = "user not found", Data = null });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<BookEntity> { Status = false, Message = "Internal server error", Data = null });
            }
        }

        [HttpGet]
        [Route("GetSuccessOrders")]
        public IActionResult GetSuccessOrders()
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString();
                token = token.Substring("Bearer ".Length);

                int userId = Convert.ToInt32(User.FindFirstValue("UserID"));

                List<OrderEntity> successfulOrders = order.SuccessOrders(token, userId);

                return Ok(successfulOrders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting successful orders: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetFailOrders")]
        public IActionResult GetFailOrders()
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString();
                token = token.Substring("Bearer ".Length);

                int userId = Convert.ToInt32(User.FindFirstValue("UserID"));

                List<OrderEntity> successfulOrders = order.failedOrders(token, userId);

                return Ok(successfulOrders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting successful orders: {ex.Message}");
            }
        }

        //[HttpPost("MakePayment")]
        //public async Task<IActionResult> MakePayment([FromBody] PayURequest paymentRequest)
        //{
        //    try
        //    {
                
        //        string paymentResponse = await order.SendPaymentRequestAsync(paymentRequest);

        //        return Ok(new ResponseModel<string> { Status = true, Message = "user details retrieved successfully", Data = paymentResponse });
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return StatusCode(500, $"Internal Server Error: {ex.Message}");
        //    }
        //}

    }
}
