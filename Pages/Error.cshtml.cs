using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razor.Pages;
using razor.Models;
using razor.Services;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : AuthPageModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<ErrorModel> _logger;

    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    public override void OnGet()
    {
        Session? s = SessionService.Get((string)this.HttpContext.Items["SessionCookie"]);
        CurrUser = AuthentificationService.Get(s.UserId);
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}

