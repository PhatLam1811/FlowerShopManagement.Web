namespace FlowerShopManagement.Application.Models;

public class RevenueModel
{
    public int _id;
    public double revenue;
}

public class OrdersCountModel
{
    public int numberOfOrders;
}

public class LowOnStocksCountModel
{
    public int amount;
}

public class ValuableCustomerModel
{
    public string _id = string.Empty;
    public int numberOfOrders;
}

public class ProfitableProductModel
{
    public string _id = string.Empty;
    public int soldNumber;
}