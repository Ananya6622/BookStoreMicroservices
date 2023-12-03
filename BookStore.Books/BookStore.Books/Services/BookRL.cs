using BookStore.Books.Context;
using BookStore.Books.Entity;
using BookStore.Books.Entity.CommandEntity;
using BookStore.Books.Entity.QueryEntity;
using BookStore.Books.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Books.Services
{
    public class BookRL : IBook
    {
        private readonly BookDbContext bookDbContext;
        private readonly IConfiguration configuration;

        public BookRL(BookDbContext bookDbContext, IConfiguration configuration)
        {
            this.bookDbContext = bookDbContext;
            this.configuration = configuration;
        }
        /// <summary>
        /// Adds a new book to the system.
        /// </summary>
        /// <param name="addUpdateBook">The book details to be added.</param>
        /// <returns>The added book details if successful, otherwise null.</returns>
        public AddUpdateBook AddBook(AddUpdateBook addUpdateBook)
        {
            try
            {
                BookEntity book = new BookEntity();
                book.BookName = addUpdateBook.BookName;
                book.Author = addUpdateBook.Author;
                book.Description = addUpdateBook.Description;
                book.Price = addUpdateBook.Price;
                book.Quantity = addUpdateBook.Quantity;
                bookDbContext.BookTable.Add(book);
                int result = bookDbContext.SaveChanges();
                if(result != null)
                {
                    return addUpdateBook;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error occurred during book addition.", ex);
            }
        }
        /// <summary>
        /// Retrieves all books from the system.
        /// </summary>
        /// <returns>A list of books if successful, otherwise null.</returns>
        public List<ViewBook> GetAllBooks()
        {
            try
            {
                List<ViewBook> viewBooks  = new List<ViewBook> ();

                List<BookEntity> bookEntities = bookDbContext.BookTable.ToList();

                foreach(var bookEntity in bookEntities)
                {
                    ViewBook books = new ViewBook();
                    books.BookId = bookEntity.BookId;
                    books.BookName = bookEntity.BookName;
                    books.Author = bookEntity.Author;
                    books.Description = bookEntity.Description;
                    books.Price = bookEntity.Price;
                    books.Quantity = bookEntity.Quantity;
                    books.UserId = bookEntity.UserId;
                    viewBooks.Add(books);
                }
                return viewBooks;
            }
            catch(Exception ex)
            {
                throw new Exception("Error occurred while retrieving all books.", ex);
            }
        }
        /// <summary>
        /// Updates the details of an existing book.
        /// </summary>
        /// <param name="addUpdateBook">The updated book details.</param>
        /// <param name="bookId">The unique identifier of the book.</param>
        /// <returns>The number of affected rows if successful, otherwise 0.</returns>
        public int UpdateBook(AddUpdateBook addUpdateBook, int bookId)
        {
            try
            {
                BookEntity result = bookDbContext.BookTable.FirstOrDefault(a =>a.BookId == bookId);
                if (result == null)
                {
                    return 0;
                }
                    result.BookName = addUpdateBook.BookName;
                    result.Author = addUpdateBook.Author;
                    result.Description = addUpdateBook.Description;
                    result.Price = addUpdateBook.Price;
                    result.Quantity = addUpdateBook.Quantity;
                    int result1 = bookDbContext.SaveChanges();
                    return result1;
                }
                
            
            catch(Exception ex)
            {
                throw new Exception("Error occurred during book update.", ex);
            }
        }
        /// <summary>
        /// Deletes a book from the system.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book.</param>
        /// <param name="userId">The unique identifier of the user deleting the book.</param>
        /// <returns>True if deletion is successful, otherwise false.</returns>
        public bool DeleteBook(int bookId, int userId)
        {
            try
            {
                var result = bookDbContext.BookTable.FirstOrDefault(a => a.BookId == bookId && a.UserId == userId);
                if(result != null)
                {
                    bookDbContext.BookTable.Remove(result);
                    bookDbContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error occurred during book deletion.", ex);
            }
        }
        /// <summary>
        /// Retrieves the details of a book by its unique identifier.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book.</param>
        /// <returns>The book details if found, otherwise null.</returns>
        public ViewBook GetAllBooksById(int bookId)
        {
            try
            {
                BookEntity bookEntity = bookDbContext.BookTable.FirstOrDefault(a => a.BookId == bookId);
                if (bookEntity == null)
                {
                    return null;
                }
                    ViewBook books = new ViewBook();
                    {
                        books.BookId = bookEntity.BookId;
                        books.BookName = bookEntity.BookName;
                        books.Author = bookEntity.Author;
                        books.Description = bookEntity.Description;
                        books.Price = bookEntity.Price;
                        books.Quantity = bookEntity.Quantity;
                        books.UserId = bookEntity.UserId;
                    };

                
                    return books;
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving book by ID.", ex);
            }
        }
        /// <summary>
        /// Retrieves a list of books that are out of stock.
        /// </summary>
        /// <returns>A list of out-of-stock books if successful, otherwise null.</returns>
        public List<ViewBook> GetOutOfStockBooks()
        {
            try
            {
                return bookDbContext.BookTable.Where(a => a.Quantity == 0).Select(a => new ViewBook
                {
                    BookId = a.BookId,
                    BookName = a.BookName,
                    Author = a.Author,
                    Description = a.Description,
                    Price = a.Price,
                    Quantity = a.Quantity,
                    UserId = a.UserId
                }).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception("Error occurred while retrieving out-of-stock books.", ex);
            }
        }
    }
}
