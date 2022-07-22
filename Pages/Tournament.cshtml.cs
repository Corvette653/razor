using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor.Models;
using razor.Services;

namespace razor.Pages
{
    public class TournamentModel : PageModel
    {
        public User CurrUser { get; set; } = new User();
        public bool MessageExists { get; set; } = false;
        public string Message { get; set; } = "";
        [BindProperty]
        public Tournament? Tournament { get; set; }
        public void OnGet(string tournament_name)
        {
            Session? s = SessionService.Get((string)HttpContext.Items["SessionCookie"]);
            CurrUser = AuthentificationService.Get(s.UserId);
            Tournament = TournamentService.Get(tournament_name);
        }
        public void OnPostStartGroups(string tournament_name)
        {
            Tournament = TournamentService.Get(tournament_name);
            MessageExists = true;
            if (Tournament == null)
            {
                Message = "Error. Groups couldn't be created";
                return;
            }
            if (Tournament.CreateGroups())
                Message = "Groups created";
            else
                Message = "Error. Groups couldn't be created";
        }
        public void OnPostSetWinner(int match_id, int match_winner_id, string tournament_name)
        {
            Match? match = MatchService.Get(match_id);
            Competitor? match_winner = CompetitorService.Get(match_winner_id);
            if (match == null || match_winner == null)
            {
                MessageExists = true;
                Message = "Error";
            }
            else
                match.SetWinner(match_winner);
            OnGet(tournament_name);
        }
        public void OnPostStartLadder(string tournament_name)
        {
            Tournament = TournamentService.Get(tournament_name);
            MessageExists = true;
            if (Tournament == null)
            {
                Message = "Error. Ladder couldn't be created";
                return;
            }
            if (Tournament.CreateLadder())
                Message = "Ladder created";
            else
                Message = "Error. Ladder couldn't be created";
        }
        public void OnPostLogOut()
        {
            AuthentificationService.LogOutUser((string)this.HttpContext.Items["SessionCookie"]);
            this.HttpContext.Response.Redirect("/");
        }
    }
}
