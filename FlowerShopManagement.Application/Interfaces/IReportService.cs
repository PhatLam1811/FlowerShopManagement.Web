using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces;

public interface IReportService
{
    // Most profitable products of day, month, year
    public List<ProfitableProductModel> GetProfitableProducts(DateTime beginDate, DateTime endDate, int limit = 5);

    // Most valuable customers of day, month, year
    public List<ValuableCustomerModel> GetValuableCustomers(DateTime beginDate, DateTime endDate, int limit = 5);

    // Total waiting/sold/... order count of day, month, year
    public int GetOrdersCount(DateTime beginDate, DateTime endDate, Status status = Status.Purchased);

    // Total revenue per selected day, month, year
    public List<double?> GetTotalRevenue(DateTime beginDate, DateTime endDate, Status status = Status.Purchased);

    // Low on stock products count
    
    // Profit of day, month, year

    // Total customers count of day, month, year
}
