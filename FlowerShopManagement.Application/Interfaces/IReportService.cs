using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces;

public interface IReportService
{
    // Waiting orders count
    
    // Total revenue per selected day, month, year
    public List<double?> GetTotalRevenue(DateTime beginDate, DateTime endDate, Status status = Status.Purchased);

    // Total sold order count of day, month, year

    // Low on stock products count

    // Potential customer
    
    // Profit of day, month, year

    // Total customers count of day, month, year

    // Profitable products list
}
