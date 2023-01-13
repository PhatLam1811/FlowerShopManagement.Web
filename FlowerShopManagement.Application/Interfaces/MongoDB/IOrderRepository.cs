using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces.MongoDB;

public interface IOrderRepository : IBaseRepository<Order>
{
    public void GetPotentialProducts(DateTime beginDate, DateTime endDate, int limit = 5);
    public void GetPotentialCustomers(DateTime beginDate, DateTime endDate, int limit = 5);
    public List<OrdersCountModel> GetOrdersCount(DateTime beginDate, DateTime endDate, Status? status = Status.Purchased);
    public List<RevenueModel> GetTotalRevenue(DateTime beginDate, DateTime endDate, string criteria = "$hour", Status status = Status.Purchased);
}
