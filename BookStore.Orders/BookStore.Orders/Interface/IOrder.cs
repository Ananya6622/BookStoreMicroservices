using BookStore.Orders.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Orders.Interface
{
    public interface IOrder
    {
        public Task<OrderEntity> AddOrder(int bookid, int quantity, string token);
        public List<OrderEntity> GetAllOrders();
    }
}
