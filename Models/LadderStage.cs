using System.ComponentModel.DataAnnotations;
using razor.Services;
namespace razor.Models;

public class LadderStage
{
    public int MatchesPlayed { get; set; } = 0;
    public Tournament Tournament { get; set; } = new Tournament();
    public bool Active { get; set; } = false;
    public IList<Competitor> Competitors { get; set; } = new List<Competitor>();
    public IList<Match> Matches { get; set; } = new List<Match>();
    public LadderStage(int k, Tournament t)
    {
        Tournament = t;
        for(int i = 0; i < k; i++)
        {
            Competitors.Add(new Competitor());
        }
        for(int i = 0; i < k; i+=2)
        {
            Match match = new Match { LadderStage = this };
            match = MatchService.Add(match);
            Matches.Add(match);
        }
    }
    public bool SetCompetitor(Competitor competitor, int position)
    {
        if (Competitors[position].Id != 0)
            return false;
        Competitors[position] = competitor;
        if(position%2 == 0)
            Matches[position / 2].SetFirstCompetitor(competitor);
        else
            Matches[position / 2].SetSecondCompetitor(competitor);
        return true;
    }
    public void PromoteCompetitor(Competitor competitor, Match match)
    {
        Tournament.PromoteToNextLadderStage(competitor, this, Matches.IndexOf(match));
    }
    public void MatchPlayed()
    {
        if (MatchesPlayed >= Matches.Count)
            return;
        MatchesPlayed++;
        if (MatchesPlayed == Matches.Count)
        {
            Active = false;
            Tournament.LadderStageFinished();
        }
    }
    public bool SetActive()
    {
        foreach(Match match in Matches)
        {
            if (match.SecondCompetitor.FullName == " ")
                match.SetWinner(match.FirstCompetitor);
        }
        Active = true;
        return true;
    }
}