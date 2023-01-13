using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces;

public interface IReportService
{
    // Waiting orders count
    
    // Total sale of day, month, year
    public void GetTotalSum(DateTime beginDate, DateTime endDate, string dateFormat, Status? status);

    // Total sold order count of day, month, year

    // Low on stock products count

    // Potential customer
    
    // Profit of day, month, year

    // Total customers count of day, month, year

    // Profitable products list
}
