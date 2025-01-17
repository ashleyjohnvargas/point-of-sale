using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using System.Threading.Tasks;
using POS1.Models;
using Microsoft.EntityFrameworkCore;

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


    }
}
