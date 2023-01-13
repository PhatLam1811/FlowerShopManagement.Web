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

    public int GetOrdersCount(DateTime beginDate, DateTime endDate, Status status = Status.Purchased)
    {
        try
        {
            // Get total number of orders per date/month/year
            var result = _orderRepository.GetOrdersCount(beginDate, endDate, status);

            return result.First().numberOfOrders;
            
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public List<double?> GetTotalRevenue(DateTime beginDate, DateTime endDate, Status status = Status.Purchased)
    {
        var option = -1;
        var initValue = 0;
        var dataSize = 0;
        var criteria = string.Empty;
        var daysBetween = endDate - beginDate;

        // Daily
        if (daysBetween.Days <= 1)
        {
            dataSize = 24; // total hours per day
            criteria = "$hour"; // group orders by hours
            option = 0;
        }

        // Monthly
        if (daysBetween.Days > 1 && daysBetween.Days <= 31)
        {
            dataSize = DateTime.DaysInMonth(beginDate.Year, beginDate.Month);
            criteria = "$dayOfMonth"; // group orders by days
            option = 1;
        }

        // Yearly
        if (daysBetween.Days > 31)
        {
            dataSize = 12; // total months per year
            criteria = "$month"; // group orders by months
            option = 2;
        }

        try
        {
            // sum up all orders "_total" per date/month/year
            var result = _orderRepository.GetTotalRevenue(beginDate, endDate, criteria, status);

            // generate data set
            var dataSet = Enumerable.Repeat<double?>(initValue, dataSize).ToList();

            foreach (var record in result)
            {
                // local time offset
                if (option == 0)
                {
                    record._id = record._id + 7;
                    if (record._id >= 24)
                        record._id = record._id - 24;
                }

                dataSet[record._id] = record.revenue;
            }

            return dataSet;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}