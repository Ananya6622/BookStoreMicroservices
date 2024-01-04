using BookStore.Orders.context;
using BookStore.Orders.Entity;
using BookStore.Orders.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
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
                BookEntity book = await AppCalling.GetBookDetailsById(bookid);
                UserEntity user = await AppCalling.GetUserDetails(token);

                OrderEntity order = new OrderEntity
                {
                    BookId = bookid,
                    Quantity = quantity,
                    BookDetails = book,
                    UserDetails = user,

                    OrderAmount = quantity * book.Price,

                };
                
                //BookEntity book = await AppCalling.GetBookDetailsById(bookid);
                //UserEntity user = await AppCalling.GetUserDetails(token);
               // order.OrderAmount = quantity * order.BookDetails.Price;
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

        public List<OrderEntity> SuccessOrders(string token, int userId)
        {
            try
            {
                List<OrderEntity> successfulOrder = orderDbContext.OrderTable.Where(x =>x.IsSuccess && x.UserId == userId).ToList();
                return successfulOrder;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred", ex);
            }
        }

        public List<OrderEntity> failedOrders(string token, int userId)
        {
            try
            {
                List<OrderEntity> unsuccessfulOrder = orderDbContext.OrderTable.Where(x => x.IsSuccess== false && x.UserId == userId).ToList();
                return unsuccessfulOrder ;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred", ex);
            }
        }
        public string GenerateTransactionID()
        {

            return Guid.NewGuid().ToString();
        }

        public PayURequest CreatePaymentRequest(
            string orderId, double orderAmount, string transactionID,
            string firstName, string lastName, string email, string phone,
            string payuKey, string payuSalt)
        {


            var paymentRequest = new PayURequest
            {
                TransactionId = orderId,
                Amount = orderAmount,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                Surl = "https://apiplayground-response.herokuapp.com/",
                Furl = "https://apiplayground-response.herokuapp.com/",
                Key = payuKey,
                Hash = "",
            };

            paymentRequest.Hash = GenerateHash(paymentRequest, payuKey, payuSalt, transactionID);

            return paymentRequest;
        }

        public string GenerateHash(PayURequest paymentRequest, string key, string salt, string transactionID)
        {
            try
            {
                string hashString =
                   $"3bJt9f|{paymentRequest.TransactionId}|{paymentRequest.Amount}|{"BookDetails"}|{paymentRequest.FirstName}|{paymentRequest.Email}|||||||||||fgDHdaLqmMhuhewf3md1k7nqBfx7B4bi";


                StringBuilder sb = new StringBuilder();
                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] hashValue = sha512.ComputeHash(Encoding.UTF8.GetBytes(hashString));
                    foreach (byte b in hashValue)
                    {
                        sb.Append($"{b:X2}");
                    }

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating hash: " + ex.Message);
            }
        }

        public async Task<string> SendPaymentRequestAsync(PayURequest paymentRequest)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var formData = new Dictionary<string, string>
            {
                { "key", "3bJt9f" },
                { "txnid", paymentRequest.TransactionId },
                { "amount", paymentRequest.Amount.ToString() },
                { "productinfo", "BookDetails" },
                { "firstname", paymentRequest.FirstName },
                {"lastname",paymentRequest.LastName },
                { "email", paymentRequest.Email },
                { "phone", paymentRequest.Phone },
                { "surl", paymentRequest.Surl },
                { "furl", paymentRequest.Furl },
                { "hash", paymentRequest.Hash }
            };

                    var content = new FormUrlEncodedContent(formData);
                    content.Headers.Clear(); 
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    var response = await httpClient.PostAsync("https://test.payu.in/_payment", content);

                    if (response.IsSuccessStatusCode)
                    {

                        var paymentresponse =  response.RequestMessage.RequestUri.ToString();
                        return paymentresponse;
                    }
                    else
                    {
                        return "Error in PayU API response";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Exception occurred: " + ex.Message;
            }
        }

    }
}
