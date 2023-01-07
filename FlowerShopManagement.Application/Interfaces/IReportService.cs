namespace FlowerShopManagement.Application.Interfaces;

public interface IReportService
{
    // Total sale of day, month, year
    public Task<double> GetTotalSale();

    // Total sold order count of day, month, year

    // Low on stock products count

    // Potential customer
    
    // Profit of day, month, year

    // Total customers count of day, month, year

    // Profitable products list
}
