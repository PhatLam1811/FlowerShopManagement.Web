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
            //UserCredential credential;

            //// Load client secrets
            //using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            //{
            //    /* The file token.json stores the user's access and refresh tokens, and is created
            //     automatically when the authorization flow completes for the first time */
            //    string credPath = "token.json";

            //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.FromStream(stream).Secrets,
            //        Scopes,
            //        "user",
            //        CancellationToken.None,
            //        new FileDataStore(credPath, true)).Result;

            //    Console.WriteLine("Credential file saved to: " + credPath);
            //}

            //// Create Gmail API service
            //var service = new GmailService(new BaseClientService.Initializer
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = ApplicationName
            //});

            GoogleCredential credential;

            try
            {
                using (var stream = new FileStream("SE100-N11-Proj-Cre.json", FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(GmailService.Scope.GmailSend)
                        .CreateWithUser("phatlam1811@gmail.com");
                }

                var service = new GmailService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "SE100-N11-Proj"
                    });

                return service;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error logging into Google Mail", e);
            }

            //return service;
        }
        catch (FileNotFoundException e)
        {
            throw new FileNotFoundException(e.Message);
        }
    }

    public bool Send()
    {
        GmailService service = GetService();

        // Create the message
        MimeMessage newMsg = new MimeMessage();

        string htmlPart = string.Empty;
        using (StreamReader streamReader = new StreamReader("Test.html"))
        {
            htmlPart = streamReader.ReadToEnd();
        }

        // Add html part to the message
        BodyBuilder bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = "Test email with html template";
        bodyBuilder.HtmlBody = htmlPart;

        newMsg.From.Add(new MailboxAddress("", "phatlam1811@gmail.com"));
        newMsg.To.Add(new MailboxAddress("", "z613zgm@gmail.com"));
        newMsg.Body = bodyBuilder.ToMessageBody();
        //newMsg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        //{
        //    Text = htmlPart
        //};

        var ms = new MemoryStream();
        newMsg.WriteTo(ms);
        ms.Position = 0;
        StreamReader sr = new StreamReader(ms);
        Message msg = new Message();
        string rawString = sr.ReadToEnd();

        msg.Raw = Base64UrlEncode(rawString);

        service.Users.Messages.Send(msg, "me").Execute();

        // Create the draft message
        //Draft draft = new Draft();
        //draft.Message = msg;
        //draft = service.Users.Drafts.Create(draft, "me").Execute();

        //service.Users.Drafts.Send(draft, "me").Execute();

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