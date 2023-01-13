using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces;

public interface IReportService
{
    // Waiting orders count

    // Most valuable customers of day, month, year
    public List<ValuableCustomerModel> GetValuableCustomers(DateTime beginDate, DateTime endDate, int limit = 5);

    // Total sold order count of day, month, year
    public int GetOrdersCount(DateTime beginDate, DateTime endDate, Status status = Status.Purchased);

    // Total revenue per selected day, month, year
    public List<double?> GetTotalRevenue(DateTime beginDate, DateTime endDate, Status status = Status.Purchased);

    // Low on stock products count
    
    // Profit of day, month, year

    // Total customers count of day, month, year

    // Profitable products list
}
