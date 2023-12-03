using BookStore.Orders.context;
using BookStore.Orders.Entity;
using BookStore.Orders.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Orders.Services
{
    public class OrderRL : IOrder
    {
        private readonly OrderDbContext orderDbContext;
        private readonly IConfiguration configuration;

        public OrderRL(OrderDbContext orderDbContext, IConfiguration configuration)
        {
            this.orderDbContext = orderDbContext;
            this.configuration = configuration;
        }
        /// <summary>
        /// Adds a new order for a book.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book.</param>
        /// <param name="quantity">The quantity of books to be ordered.</param>
        /// <param name="token">The user's authentication token.</param>
        /// <returns>The added order entity if successful, otherwise null.</returns>
        public async Task<OrderEntity> AddOrder(int bookid,int quantity, string token)
        {
            try
            {
                OrderEntity order = new OrderEntity
                {
                    BookId = bookid,
                    Quantity = quantity,

                };
                
                order.BookDetails = await AppCalling.GetBookDetailsById(bookid);
                order.UserDetails = await AppCalling.GetUserDetails(token);
                order.OrderAmount = quantity * order.BookDetails.Price;
                orderDbContext.OrderTable.Add(order);
                int result = orderDbContext.SaveChanges();
                if(result != null)
                {
                    return order;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error occurred during order addition.", ex);
            }
        }
        /// <summary>
        /// Retrieves a list of all orders.
        /// </summary>
        /// <returns>A list of order entities if successful, otherwise null.</returns>
        public List<OrderEntity> GetAllOrders()
        {
            try
            {
                return orderDbContext.OrderTable.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving all orders.", ex);
            }
        }
    }
}
