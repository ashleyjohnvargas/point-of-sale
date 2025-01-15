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
        private readonly ApplicationDbContext _context;

        public EcommerceService(HttpClient httpClient, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }
       
    }
}
