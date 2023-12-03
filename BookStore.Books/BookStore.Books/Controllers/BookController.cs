using BookStore.Books.Entity;
using BookStore.Books.Entity.CommandEntity;
using BookStore.Books.Entity.QueryEntity;
using BookStore.Books.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BookStore.Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBook book;
        public BookController(IBook book)
        {
            this.book = book;
        }

        [HttpPost("AddBook")]

        public IActionResult AddBook(AddUpdateBook addUpdateBook)
        {
            try
            {
                var result = book.AddBook(addUpdateBook);
                if (result != null)
                {
                    return this.Ok(new ResponseModel<AddUpdateBook> { Status = true, Message = "added book successfully", Data = result });
                }
                else
                {
                    return this.BadRequest(new { Status = false, message = "unable to add book" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("GetAllBooks")]
        public IActionResult GetAllBooks()
        {
            try
            {
                var result = book.GetAllBooks();
                if (result != null)
                {
                    return this.Ok(new ResponseModel<List<ViewBook>> { Status = true, Message = "retrieved all books", Data = result });
                }
                else
                {
                    return this.BadRequest(new { Status = false, message = "unable to get books" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPut("UpdateBooks")]
        public IActionResult UpdateBooks(AddUpdateBook addUpdateBook, int bookId)
        {
            try
            {
                var result = book.UpdateBook(addUpdateBook, bookId);
                if (result != null)
                {
                    return this.Ok(new ResponseModel<int> { Status = true, Message = "updated books successfully", Data = result });
                }
                else
                {
                    return this.BadRequest(new { Status = false, message = "unable to update books" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpDelete("DeleteBook")]
        public IActionResult DeleteBook(int bookid, int userId)
        {
            try
            {
                bool result = book.DeleteBook(bookid, userId);
                if (result != null)
                {
                    return this.Ok(new ResponseModel<bool> { Status = true, Message = "deleted book successfully", Data = result });
                }
                else
                {
                    return this.BadRequest(new { Status = false, message = "unable to delete book" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet("GetBooksById")]
        public IActionResult GetBooksById(int bookId)
        {
            try
            {
                var result = book.GetAllBooksById(bookId);
                if (result != null)
                {
                    return this.Ok(new ResponseModel <ViewBook> { Status = true, Message = "retrieved all books by id", Data = result });
                }
                else
                {
                    return this.BadRequest(new { Status = false, message = "unable to get books by id" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("OutOfStock")]
        public IActionResult GetOutOfStockBooks()
        {
            try
            {
                var result = book.GetOutOfStockBooks();
                if(result != null)
                {
                    return this.Ok(new ResponseModel<List<ViewBook>> { Status = true, Message = "retrieved all out of stock books", Data = result });
                }
                else
                {
                    return this.BadRequest(new { status = false, message = "unable to retrieve books" });
                }
            }
            catch(Exception ex)
            {
                return this.BadRequest(new {success  = false, message = ex.Message});
            }
        }
    }
}
