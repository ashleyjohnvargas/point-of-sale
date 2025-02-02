using POS1.Models;

namespace POS1.Services
{
    public class InventoryService
    {
        private readonly HttpClient _httpClient;

        public InventoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task UpdateProductStockAsync(List<ProductStockUpdateModel> stockUpdates)
        {
            var response = await _httpClient.PostAsJsonAsync("https://gizmodeinventorysystem2.azurewebsites.net/api/ProductsApi/UpdateStock", stockUpdates);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update product stock. Status Code: {response.StatusCode}, Message: {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}
