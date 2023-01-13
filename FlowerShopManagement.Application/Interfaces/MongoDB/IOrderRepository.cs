﻿using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces.MongoDB;

public interface IOrderRepository : IBaseRepository<Order>
{
    public void TotalCount(DateTime beginDate, DateTime endDate, string dateFormat = "$hour", Status? status = Status.Purchased);
    public void TotalSum(DateTime beginDate, DateTime endDate, string dateFormat = "$hour", Status status = Status.Purchased);
}
