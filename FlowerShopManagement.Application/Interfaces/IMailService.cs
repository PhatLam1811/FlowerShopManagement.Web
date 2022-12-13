using FlowerShopManagement.Application.Models;
using MimeKit;

namespace FlowerShopManagement.Application.Interfaces;

public interface IMailService
{
    public Task<bool> Send(MimeMessage mimeMessage);
    public MimeMessage CreateMimeMessage(SupplyFormModel supplyForm, string? htmlPath = null);
}
