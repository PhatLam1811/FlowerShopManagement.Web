using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Text;

namespace FlowerShopManagement.Application.Services;

public class EmailService : IEmailService
{
    public async Task<bool> Send(MimeMessage mimeMessage)
    {
        try
        {
            // Generate smtp client instance
            using var smtp = new SmtpClient();

            // Establish & authorize a connection to gmail smtp server
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("phatlam1811@gmail.com", "xhbstfiflmipyjfq");

            // Asynchronous mime message send
            await smtp.SendAsync(mimeMessage);

            // Disconnect the service
            smtp.Disconnect(true);

            // Successfully sent the mail
            return true;
        }
        catch
        {
            // Failed to send the mail
            return false;
        }
    }

    public MimeMessage CreateMimeMessage(SupplyFormModel supplyForm, string? htmlPath = null)
    {
        // Create a mime message instance
        var mimeMessage = new MimeMessage();

        // Configure header
        mimeMessage.From.Add(MailboxAddress.Parse(supplyForm.From));
        mimeMessage.Subject = supplyForm.Subject;

        // Set message's bcc
        foreach (string address in supplyForm.To)
            mimeMessage.Bcc.Add(MailboxAddress.Parse(address));

        // Read html template
        string? htmlBody = CreateHtmlBody(htmlPath);

        // Build message body
        BodyBuilder bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = supplyForm.Content;
        bodyBuilder.HtmlBody = htmlBody;

        mimeMessage.Body = bodyBuilder.ToMessageBody();

        return mimeMessage;
    }

    private string? CreateHtmlBody(string? htmlPath)
    {
        if (htmlPath == null) return null;

        StringBuilder stringBuilder = new StringBuilder();

        using (var reader = new StreamReader(htmlPath))
        {
            while (!reader.EndOfStream)
                stringBuilder.AppendLine(reader.ReadLine());
        }

        return stringBuilder.ToString();
    }
}
