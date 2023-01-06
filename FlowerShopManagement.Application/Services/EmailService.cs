using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace FlowerShopManagement.Application.Services;

public class EmailService : IEmailService
{
    public async Task<bool> SendAsync(MimeMessage mimeMessage)
    {
        try
        {
            // Establish & authorize a connection to gmail smtp server
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("phatlam1811@gmail.com", "xhbstfiflmipyjfq");

            // Asynchronous mime message send
            await smtp.SendAsync(mimeMessage);

            // Disconnect the service
            smtp.Disconnect(true);

            // Successfully sent the mail
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public MimeMessage CreateMimeMessage(SupplyFormModel supplyForm)
    {
        MimeMessage mimeMessage = new MimeMessage();

        // Configure header
        mimeMessage.From.Add(MailboxAddress.Parse(supplyForm.From));
        mimeMessage.Subject = supplyForm.Subject;

        // Set message's bcc
        foreach (var email in supplyForm.To)
        {
            mimeMessage.Bcc.Add(MailboxAddress.Parse(email));
        }

        // Build message body
        BodyBuilder builder = new BodyBuilder();
        builder.TextBody = supplyForm.TextPart;
        builder.HtmlBody = supplyForm.HtmlPart;

        mimeMessage.Body = builder.ToMessageBody();

        return mimeMessage;
    }
}
