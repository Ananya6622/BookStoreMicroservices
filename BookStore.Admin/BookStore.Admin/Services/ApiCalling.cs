using BookStore.Admin.Entity;
using BookStore.Admin.Entity.QueryEntity;
using BookStore.Admin.Interfaces;
using BookStore.Books.Entity;

using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookStore.Admin.Services
{
    //Create an api which would retrive all the out of stock books.

    //Consume this api in admin project
    public class ApiCalling
    {
        public async Task<ViewBook> GetOutOfStockBooks()
        {
            HttpClient httpClient = new HttpClient();
            string url = "https://localhost:44305/api/Book/OutOfStock";

            HttpResponseMessage responseObj = await httpClient.GetAsync(url);

            if(responseObj != null)
            {
                string Content  = await responseObj.Content.ReadAsStringAsync();
                ResponseModel<ViewBook> response = JsonConvert.DeserializeObject<ResponseModel<ViewBook>>(Content);
                return response.Data;
            }
            return null;
        }
    }
}
