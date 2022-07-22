using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace razor.Pages;
using razor.Models;
using razor.Services;
public class add_tournamentModel : AuthPageModel
{
    public List<Competitor> Competitors { get; set; } = new List<Competitor>();
    [BindProperty]
    public Tournament NewTournament { get; set; } = new Tournament();
    [BindProperty]
    public int[] NewCompetitorsIds { get; set; } = Array.Empty<int>();
    public override void OnGet()
    {
        Session? s = SessionService.Get((string)this.HttpContext.Items["SessionCookie"]);
        CurrUser = AuthentificationService.Get(s.UserId);
        if (CurrUser == null || CurrUser.Role == UserRole.User)
            this.HttpContext.Response.Redirect("/error");
        Competitors = CompetitorService.GetAll();
    }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();
        foreach (int competitorId in NewCompetitorsIds)
        {
            Competitor competitor = CompetitorService.Get(competitorId);
            if(competitor != null)
                NewTournament.AddCompetitor(competitor);
        }
        if (TournamentService.Add(NewTournament) == null)
        {
            MessageExists = true;
            Message = "Tournament with this name currently exists.";
            return Page();
        }

        return Redirect("/");
    }
}
