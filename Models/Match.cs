using System.ComponentModel.DataAnnotations;

namespace razor.Models;

public class Match
{
    public int Id { get; set; }
    public Group? Group { get; set; }
    public LadderStage? LadderStage { get; set; }
    public Competitor FirstCompetitor { get; set; } = new Competitor();
    public Competitor SecondCompetitor { get; set; } = new Competitor();
    public bool Winner { get; set; }
    public bool Played { get; set; } = false;
    public void SetFirstCompetitor(Competitor competitor)
    {
        FirstCompetitor = competitor;
        competitor.AddMatch(this);
    }
    public void SetSecondCompetitor(Competitor competitor)
    {
        SecondCompetitor = competitor;
        competitor.AddMatch(this);
    }
    public Competitor? GetWinner()
    {
        if (Played == false)
            return null;
        if (Winner)
            return FirstCompetitor;
        return SecondCompetitor;
    }
    public Competitor? GetLoser()
    {
        if (Played == false)
            return null;
        if (Winner)
            return SecondCompetitor;
        return FirstCompetitor;
    }
    public bool SetWinner(Competitor winner)
    {
        if (Played)
            return false;
        if (FirstCompetitor.Id == winner.Id)
            Winner = true;
        else if (SecondCompetitor.Id == winner.Id)
            Winner = false;
        else
            return false;

        Played = true;
        if (Group != null)
        {
            Group.AddPoints(winner, 3);
            Group.SortGroup();
            Group.MatchPlayed();
        }
        if(LadderStage != null)
        {
            LadderStage.PromoteCompetitor(winner, this);
            LadderStage.MatchPlayed();
        }
        return true;
    }
}