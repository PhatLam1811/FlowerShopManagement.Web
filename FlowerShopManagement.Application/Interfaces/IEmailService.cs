using FlowerShopManagement.Application.Models;
using MimeKit;

namespace FlowerShopManagement.Application.Interfaces;

public interface IEmailService
{
    public Task<bool> SendAsync(MimeMessage mimeMessage);
    public bool Send(MimeMessage mimeMessage);
    public MimeMessage CreateMimeMessage(SupplyFormModel supplyForm);
}
