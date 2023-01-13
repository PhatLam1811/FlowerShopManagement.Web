using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER SERVICES **************
// - New adjustments could be made in future updates
// - This should be a use case logic contains the CRUD operation of Customer & Cart objects 

namespace FlowerShopManagement.Application.Services;

public class SaleService : ISaleService
{
    //List<Order> _orders; 

    // APPLICATION SERVICES (USE CASES)
    public SaleService()
    {

    }


    public async Task<bool> CreateOfflineOrder(OrderModel order, UserModel user, IOrderRepository orderRepository,
        IUserRepository userRepository, IProductRepository productRepository)
    {
        if (order.Products == null || user == null || order.Products.Count > 20 || order.Products.Count == 0) return false;

        //Create OrderEntity object
        var newOrder = order.ToEntity();
        var cus = await userRepository.GetByEmailOrPhoneNb(user.PhoneNumber);
        if (cus != null)
        {
            newOrder._accountID = cus._id;
            newOrder._phoneNumber = cus.phoneNumber;
            newOrder._customerName = cus.name;
        }
        else
        {
            //Customer is a passenger
            newOrder._phoneNumber = user.PhoneNumber;
            newOrder._customerName = user.Name;
        }

        if (newOrder != null && newOrder._id != null)
        {
            List<Product> updateProductList = new List<Product>();

            if (newOrder._products != null)
            {
                // reduce the amount of product before updating
                foreach (var item in newOrder._products)
                {
                    if (item != null && item._id != null)
                    {
                        var product = await productRepository.GetById(item._id);
                        //Amount unavaibale => false
                        if (product == null || product._amount < item._amount)
                            return false;
                        product._amount -= item._amount;
                        newOrder._total += product._amount * product._uniPrice;
                        //Add to updateProList
                        updateProductList.Add(product);
                    }

                }
            }
            // Updating...
            foreach (var item in updateProductList)
            {
                if (item != null && item._id != null)
                {
                    var updateResult = await productRepository.UpdateById(item._id, item);
                    if (!updateResult)
                        return false;
                }
            }
            newOrder._date = DateTime.Now;
            // Successful case happens
            newOrder._status = Status.Paying;//On charging
            var result = await orderRepository.Add(newOrder);
            return result;

        }
        return false;
    }

    public async Task<bool> VerifyOnlineOrder(OrderModel order, IOrderRepository orderRepository, IProductRepository productRepository)
    {
        //update
        if (order == null || order.Id == null || order.Products == null) return false;

        var ids = order.Products.Select(i => i.Id).ToList();
        if (ids == null) return false;
        var currentProducts = await productRepository.GetProductsById(ids);
        //Check the amount is avaiable before updating
        if (currentProducts == null) return false;

        List<Product> updateProductList = new List<Product>();

        foreach (var item in order.Products)
        {
            if (item != null && item.Id != null)
            {
                var product = currentProducts.FirstOrDefault(i => i._id == item.Id);
                //Amount unavaibale => false
                if (product == null || product._amount < item.Amount)
                {
                    order.Status = Status.OutOfStock;
                    return false;
                }

                product._amount -= item.Amount;

            }

        }
        //Updating product
        foreach (var item in currentProducts)
        {
            if (item != null && item._id != null)
            {
                var updateResult = await productRepository.UpdateById(item._id, item);
                if (updateResult == false)
                    return false;
            }
        }
        order.Status = Status.Delivering;//Delivering
        return await orderRepository.UpdateById(order.Id, order.ToEntity());
    }

    public async Task<List<OrderModel>> GetUpdatedOrders(IOrderRepository orderRepository)
    {
        List<Order> orders = await orderRepository.GetAll();
        if (orders == null) return null;
        List<OrderModel> orderMs = new List<OrderModel>();

        foreach (var o in orders)
        {
            orderMs.Add(new OrderModel(o));
        }
        return orderMs;
    }
    public async Task<OrderModel> GetADetailOrder(string id, IOrderRepository orderRepository)
    {
        Order? order = await orderRepository.GetById(id);
        if (order == null) return new OrderModel();
        return new OrderModel(order);
    }

    public void PickItems(List<string> ids, List<int> amounts, List<ProductModel> currentProducts, List<ProductModel> allProductModels)
    {
        for (int i = 0; i < amounts.Count && i < ids.Count; i++)
        {
            if (amounts[i] == 0 || ids[i] == "") continue;

            ProductModel? p = currentProducts.Find(o => o.Id != null && o.Id.Equals(ids[i]));
            if (p != null)
            {
                p.Amount += amounts[i];
            }
            else
            {
                ProductModel? product = allProductModels.FirstOrDefault(o => o.Id != null && o.Id.Equals(ids[i]));
                if (product != null)
                {
                    product.Amount = amounts[i];
                    currentProducts.Add(product);
                }
            }
        }
    }


}
