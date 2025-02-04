using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using System.Threading.Tasks;
using POS1.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace POS1.Services
{
    public class EcommerceService
    {
        private readonly HttpClient _httpClient;
        
        public EcommerceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }

        // Get order items from the Ecommerce of the order with the order status of "Order Confirmed".
        // THe orders with "Order Confirmed" status are the orders with PaymentMethod of either Ewallet or Bank

        public async Task<List<OrderItemCopy>> GetOrderItems(int orderId)
        {
            try
            {
                // Call the Ecommerce API endpoint to get the order items
                var response = await _httpClient.GetAsync($"api/OrdersApi/GetAllOrderItems/{orderId}");

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Deserialize the response content into a list of OrderItemCopy
                var orderItems = await response.Content.ReadFromJsonAsync<List<OrderItemCopy>>();

                // Return the list or an empty list if the response is null
                return orderItems ?? new List<OrderItemCopy>();
            }
            catch (HttpRequestException ex)
            {
                // Log the error (optional) and rethrow or handle as needed
                Console.WriteLine($"An error occurred while fetching order items: {ex.Message}");
                throw;
            }
        }



        public async Task UpdateOrderInEcommerce(OrderRefundModel model)
        {

            //var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Sending the HTTP POST request to update the order
            var response = await _httpClient.PostAsJsonAsync($"api/OrdersApi/UpdateOrder", model);

            if (response.IsSuccessStatusCode)
            {
                // Successfully updated the order in Ecommerce
                // You can handle logging or additional logic here if needed
            }
            else
            {
                // Log or handle the failure case
                throw new Exception("Failed to update the order in Ecommerce.");
            }
        }


        public async Task CancelOrderInEcommerce(int orderId)
        {
            try
            {
                // Sending a POST request to the API to cancel the order
                var response = await _httpClient.PostAsJsonAsync($"api/OrdersApi/CancelOrder/{orderId}", new { OrderId = orderId });

                if (response.IsSuccessStatusCode)
                {
                    // Successfully cancelled the order in Ecommerce
                    // You can handle logging or additional logic here if needed
                }
                else
                {
                    // Log or handle the failure case
                    throw new Exception("Failed to cancel the order in Ecommerce.");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the error (optional) and rethrow or handle as needed
                Console.WriteLine($"An error occurred while cancelling the order: {ex.Message}");
                throw;
            }
        }



    }
}
