using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Services;

public class ReportService : IReportService
{
    private readonly IOrderRepository _orderRepository;

    public ReportService(IProductRepository productRepository, IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public void GetTotalSum(DateTime beginDate, DateTime endDate, string dateFormat, Status? status)
    {
        try
        {
            _orderRepository.PotentialCustomer(beginDate, endDate);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
