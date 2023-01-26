using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace FlowerShopManagement.Application.Services;

public class EmailService : IEmailService
{
    private const string sendEmail = "phatlam1811@gmail.com";

    public async Task<bool> SendAsync(MimeMessage mimeMessage)
    {
        try
        {
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(sendEmail, "xhbstfiflmipyjfq");

            await smtp.SendAsync(mimeMessage);

            smtp.Disconnect(true);

            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public bool Send(MimeMessage mimeMessage)
    {
        try
        {
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(sendEmail, "xhbstfiflmipyjfq");

            smtp.Send(mimeMessage);

            smtp.Disconnect(true);

            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public MimeMessage CreateMimeMessage(SupplyFormModel supplyForm)
    {
        var mimeMessage = new MimeMessage();

        // Configure header
        mimeMessage.From.Add(MailboxAddress.Parse(sendEmail));
        mimeMessage.Subject = "Supply request from Dallas";

        // Set message's bcc
        foreach (var suppliers in supplyForm.Suppliers)
        {
            mimeMessage.Bcc.Add(MailboxAddress.Parse(suppliers.email));
        }

        // Build message body
        BodyBuilder builder = new BodyBuilder();
        builder.HtmlBody = supplyForm.HtmlPart;

        mimeMessage.Body = builder.ToMessageBody();

        return mimeMessage;
    }
}
