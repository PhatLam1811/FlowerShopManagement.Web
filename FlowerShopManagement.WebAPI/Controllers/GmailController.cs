using FlowerShopManagement.Infrustructure.Google.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GmailController : Controller
{
    private readonly IGmailServices _gmailServices;

    public GmailController(IGmailServices gmailServices)
    {
        _gmailServices = gmailServices;
    }

    [HttpPost]
    public bool SendGmail()
    {
        return _gmailServices.Send();
    }
}
