using BookStore.Books.Entity;
using BookStore.Books.Entity.CommandEntity;
using BookStore.Books.Entity.QueryEntity;
using System.Collections.Generic;

namespace BookStore.Books.Interface
{
    public interface IBook
    {
        public AddUpdateBook AddBook(AddUpdateBook addUpdateBook);
        public List<ViewBook> GetAllBooks();
        public int UpdateBook(AddUpdateBook addUpdateBook, int bookId);
        public bool DeleteBook(int bookId, int userId);
        public ViewBook GetAllBooksById(int bookId);

        public List<ViewBook> GetOutOfStockBooks();
    }
}
