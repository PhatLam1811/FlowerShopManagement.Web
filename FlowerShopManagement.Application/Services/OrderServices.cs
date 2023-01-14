using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER SERVICES **************
// - New adjustments could be made in future updates
// - This should be a use case logic contains the CRUD operation of Customer & Cart objects 

namespace FlowerShopManagement.Core.Services;

public class OrderServices : IOrderServices
{
    public async Task<bool> CreateOrder(OrderModel order, UserModel currentUser, IOrderRepository orderRepository,
        IUserRepository userRepository, IProductRepository productRepository)
    {
        //Create OrderEntity object
        var newOrder = order.ToEntity();
        var cus = await userRepository.GetByEmailOrPhoneNb(currentUser.PhoneNumber);
        if (cus != null)
        {
            newOrder._accountID = cus._id;
            newOrder._phoneNumber = cus.phoneNumber;
            newOrder._customerName = cus.name;
        }
        else
        {
            return false;
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
                        if (product._amount < item._amount) return false;
                        product._amount -= item._amount;
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
            // Successful case happens
            newOrder._status = Status.Waiting;//On waiting for confirm
            var result = await orderRepository.Add(newOrder);
            return result;

        }
        return false;
    }

    public void SetDeliveryMethod(OrderModel order, string type)
    {
        //this's also bullshit
        order.DeliveryMethod = (DeliverryMethods)Enum.Parse(typeof(DeliverryMethods), type);
    }
    public void SetAmount(OrderModel order, int amount)
    {
        //bullshit
    }
    public void SetNote(OrderModel order, string note)
    {
        //bullshit
    }
    public void SetAddress(OrderModel order, string address)
    {
        //bullshit
    }


}
