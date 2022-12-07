using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Models;

public class OrderModel
{
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
	public DeliverryMethods DeliveryMethod { get; set; }
	public List<Product> Products { get; set; }
	public string? Notes { get; set; }
	public OrderModel(Order entity)
    {
        PhoneNumber = entity._phoneNumber;
        FullName = entity._customerName;
		DeliveryMethod = entity._deliveryMethod;
        Products = entity._products;
        Notes = entity._notes;
    }

    public Order ToEntity()
    {
		Order entity = new Order();
        entity._phoneNumber= PhoneNumber;
        entity._customerName = FullName;
        entity._deliveryMethod = DeliveryMethod;
        entity._products = Products;
        entity._notes = Notes;
        return entity;
    }
}


