using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razor.Pages;
using razor.Models;
using razor.Services;

public class IndexModel : AuthPageModel
{
    private readonly ILogger<IndexModel> _logger;
    public List<Tournament> Tournaments { get; set; } = new List<Tournament>();
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }
    public override void OnGet()
    {
        Session? s = SessionService.Get((string)this.HttpContext.Items["SessionCookie"]);
        CurrUser = AuthentificationService.Get(s.UserId);

        Tournaments = TournamentService.GetAll();
    }
}
