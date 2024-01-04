using BookStore.Orders.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Orders.Interface
{
    public interface IOrder
    {
        public Task<OrderEntity> AddOrder(int bookid, int quantity, string token);
        public List<OrderEntity> GetAllOrders();
        public List<OrderEntity> SuccessOrders(string token, int userId);
        public List<OrderEntity> failedOrders(string token, int userId);
        public Task<string> SendPaymentRequestAsync(PayURequest paymentRequest);
        public string GenerateTransactionID();
        public string GenerateHash(PayURequest paymentRequest, string key, string salt, string transactionID);
    }
}
