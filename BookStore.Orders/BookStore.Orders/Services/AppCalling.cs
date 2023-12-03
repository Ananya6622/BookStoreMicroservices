
using BookStore.Orders.Entity;
using BookStore.Orders.Interface;
using BookStore.Orders.Entity;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace BookStore.Orders.Services
{
    public class AppCalling 
    {
        //// <summary>
        /// Retrieves book details by book ID from the Book API.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book.</param>
        /// <returns>The book details if successful, otherwise null.</returns>
        public static async Task<BookEntity> GetBookDetailsById(int bookId)
        {
            HttpClient client = new HttpClient();

            string url = "https://localhost:44305/api/Book/GetBooksById?bookId";

            HttpResponseMessage responseobj = await client.GetAsync(url + bookId);

            if (responseobj.IsSuccessStatusCode)
            {
                String Content = await responseobj.Content.ReadAsStringAsync();
                ResponseModel<BookEntity> response = JsonConvert.DeserializeObject<ResponseModel<BookEntity>>(Content);
                return response.Data;
                BookEntity book = JsonConvert.DeserializeObject<BookEntity>(response.Data.ToString());

                return book; 
            }
            return null;
        }
        /// <summary>
        /// Retrieves user details by user token from the User API.
        /// </summary>
        /// <param name="token">The user's authentication token.</param>
        /// <returns>The user details if successful, otherwise null.</returns>
        public static async Task<UserEntity> GetUserDetails(string token)
        {
            HttpClient httpClient = new HttpClient();
            string url = "https://localhost:44391/api/User/MyProfile?userId=";

            HttpResponseMessage response = await httpClient.GetAsync(url);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);


            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                ResponseModel<UserEntity> response1 = JsonConvert.DeserializeObject< ResponseModel<UserEntity>>(content);
                return response1.Data;
                UserEntity user = JsonConvert.DeserializeObject<UserEntity>(response1.Data.ToString());
                return user;
            }
            return null;
        }
    }
}
