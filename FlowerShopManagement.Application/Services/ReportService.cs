using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.MongoDB.Interfaces;

namespace FlowerShopManagement.Application.Services;

public class ReportService : IReportService
{
    private readonly IProductRepository _productRepository;

    public ReportService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<double> GetTotalSale()
    {

    }
}
