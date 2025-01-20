namespace POS1.Models
{
    public class DashboardViewModel
    {
        // Sales data
        public decimal SalesToday { get; set; }
        public decimal SalesThisWeek { get; set; }
        public decimal SalesThisMonth { get; set; }
        public decimal SalesThisYear { get; set; }

        // Orders per status
        public List<OrderStatusCount> OrdersStatusToday { get; set; }
        public List<OrderStatusCount> OrdersStatusMonth { get; set; }

        // Top selling products
        public List<TopSellingProduct> TopSellingProducts { get; set; }

        // Top categories
        public List<TopCategory> TopCategories { get; set; }

        // Payment methods
        public List<PaymentMethodCount> PaymentMethods { get; set; }
    }
}
