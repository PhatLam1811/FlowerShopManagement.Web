using FlowerShopManagement.Infrustructure.Google.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;

namespace FlowerShopManagement.Infrustructure.Google.Implementations;

public class GmailServices : IGmailServices
{
    static string[] Scopes = { GmailService.Scope.GmailSend };
    static string ApplicationName = "Dallas Flower Shop Website";
 
    #region sample_code
    //public bool SendGmail(VerificationMailTemplate data)
    //{
    //    try
    //    {
    //        string[] Scopes = { GmailService.Scope.GmailSend };
    //        UserCredential credential;

    //        using (var stream = new FileStream(
    //            "./client_secret.json",
    //            FileMode.Open,
    //            FileAccess.Read
    //        ))
    //        {
    //            string credPath = "token_Send.json";
    //            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
    //                         GoogleClientSecrets.FromStream(stream).Secrets,
    //                          Scopes,
    //                          "user",
    //                          CancellationToken.None,
    //                          new FileDataStore(credPath, true)).Result;
    //            //Console.WriteLine("Credential file saved to: " + credPath);
    //        }

    //        // Create Gmail API service.
    //        var service = new GmailService(new BaseClientService.Initializer()
    //        {
    //            HttpClientInitializer = credential,
    //            ApplicationName = "mycodebit",
    //        });

    //        //Parsing HTML 
    //        string message = $"To: {data._toAddress}\r\nSubject: {data._title}\r\nContent-Type: text/html;charset=utf-8\r\n\r\n{data._body}";
    //        var newMsg = new Message();
    //        newMsg.Raw = this.Base64UrlEncode(message.ToString());
    //        Message response = service.Users.Messages.Send(newMsg, "me").Execute();

    //        if (response != null)
    //            return true;
    //        else
    //            return false;
    //    }
    //    catch (Exception e)
    //    {
    //        return false;
    //    }
    //}
    #endregion
    
    public GmailService GetService()
    {
        try
        {
            UserCredential credential;

            // Load client secrets
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                /* The file token.json stores the user's access and refresh tokens, and is created
                 automatically when the authorization flow completes for the first time */
                string credPath = "token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service
            var service = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            return service;
        }
        catch (FileNotFoundException e)
        {
            throw new FileNotFoundException(e.Message);
        }
    }

    public bool Send()
    {
        GmailService service = GetService();

        MimeMessage newMsg = new MimeMessage();

        newMsg.From.Add(new MailboxAddress("", "phatlam1811@gmail.com"));
        newMsg.To.Add(new MailboxAddress("", "z613zgm@gmail.com"));
        newMsg.Body = new TextPart("html")
        {
            Text = "2nd Test Email"
        };

        var ms = new MemoryStream();
        newMsg.WriteTo(ms);
        ms.Position = 0;
        StreamReader sr = new StreamReader(ms);
        Message msg = new Message();
        string rawString = sr.ReadToEnd();

        byte[] raw = System.Text.Encoding.UTF8.GetBytes(rawString);
        msg.Raw = Base64UrlEncode(rawString);

        service.Users.Messages.Send(msg, "me").Execute();

        return true;
    }

    private string Base64UrlEncode(string input)
    {
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);

        // Special "url-safe" base64 encode
        return Convert.ToBase64String(inputBytes)
          .Replace('+', '-')
          .Replace('/', '_')
          .Replace("=", "");
    }
}