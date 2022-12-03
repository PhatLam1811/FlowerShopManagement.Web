using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using FlowerShopManagement.Application.Templates;

namespace FlowerShopManagement.Infrustructure.Mail;

public class MailServices
{
    public async Task<bool> Send(SupplyRequestFormModel requestFormModel)
    {
        //string htmlPart = string.Empty;
        //using (StreamReader streamReader = new StreamReader(html))
        //{
        //    htmlPart = streamReader.ReadToEnd();
        //}

        // Create Message
        var mimeMessage = CreateMimeMessage(requestFormModel);
       
        // Send
        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("phatlam1811@gmail.com", "xhbstfiflmipyjfq");
        await smtp.SendAsync(mimeMessage);
        smtp.Disconnect(true);

        return true;
    }

    private MimeMessage CreateMimeMessage(SupplyRequestFormModel requestFormModel)
    {
        // Build message body
        BodyBuilder bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = requestFormModel.TextPart;
        bodyBuilder.HtmlBody = requestFormModel.HtmlPart;

        // Create mime message
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(MailboxAddress.Parse(requestFormModel.From));
        mimeMessage.To.Add(MailboxAddress.Parse(requestFormModel.To));
        mimeMessage.Subject = requestFormModel.Subject;
        mimeMessage.Body = bodyBuilder.ToMessageBody();

        return mimeMessage;
    }
}
