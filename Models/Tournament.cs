using System.ComponentModel.DataAnnotations;
using razor.Services;
namespace razor.Models;
public class Tournament
{
    public int Id { get; set; }
    public TournamentState State { get; set; } = TournamentState.Scheduled;
    public bool GroupStage { get; set; } = false;
    public int NumberOfGroupWinners { get; set; } = 2;
    public int NumberOfPlayersInGroup { get; set; } = 4;
    public IList<Competitor> Competitors { get; set; } = new List<Competitor>();
    public IList<Group> Groups { get; set; } = new List<Group>();
    public int FinishedGroups { get; set; } = 0;
    public IList<LadderStage> Ladder { get; set; } = new List<LadderStage>();
    public int FinishedLadderStages { get; set; } = 0;
    public Competitor? Winner { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public bool AddCompetitor(Competitor newCompetitor)
    {
        if (State != TournamentState.Scheduled)
            return false;
        Competitors.Add(newCompetitor);
        newCompetitor.AddTournament(this);
        return true;
    }
    public bool CreateGroups()
    {
        if (State != TournamentState.Scheduled || GroupStage == false)
            return false;
        State = TournamentState.GroupsPending;
        char name = 'A';
        List<Competitor> allCompetitors = new(Competitors);
        Random random = new Random();
        while (allCompetitors.Count > 0)
        {
            Group g = new Group { Name = name, Tournament = this };
            for (int i = 0; i < NumberOfPlayersInGroup && allCompetitors.Count > 0; i++)
            {
                Competitor c = allCompetitors[random.Next(allCompetitors.Count)];
                allCompetitors.Remove(c);
                g.Competitors.Add(new CompPoint(c, 0));
            }
            g.CreateMatches();
            g = GroupService.Add(g);
            Groups.Add(g);
            name++;
        }
        return true;
    }
    public void GroupFinished()
    {
        if (FinishedGroups >= Groups.Count)
            return;
        FinishedGroups++;
        if (FinishedGroups == Groups.Count)
        {
            State = TournamentState.GroupsFinished;
        }
    }
    public bool CreateLadder()
    {
        if (GroupStage && State != TournamentState.GroupsFinished || GroupStage == false && State != TournamentState.Scheduled)
            return false;
        State = TournamentState.Ladder;

        List<Competitor>? allCompetitors;
        if (GroupStage == true)
        {
            allCompetitors = new List<Competitor>();
            foreach (Group group in Groups)
            {
                for (int i = 0; i < NumberOfGroupWinners && i < NumberOfPlayersInGroup; i++)
                    allCompetitors.Add(group.Competitors[i].Competitor); // looking for all competitors who takes part in the ladder
            }
        }
        else
            allCompetitors = new(Competitors);

        int k = 2;
        while (k < allCompetitors.Count)
            k *= 2;
        while (k > 1)
        {
            Ladder.Add(new LadderStage(k, this)); // creating blank ladderStages
            k /= 2;
        }

        Random random = new Random();
        k = 0;
        while (allCompetitors.Count > 0)
        {
            Competitor c = allCompetitors[random.Next(allCompetitors.Count)];
            allCompetitors.Remove(c);
            Ladder[0].SetCompetitor(c, k);
            k++;
        }
        Ladder[0].SetActive();
        return true;
    }
    public void PromoteToNextLadderStage(Competitor competitor, LadderStage stage, int position)
    {
        int s = Ladder.IndexOf(stage) + 1;
        if (s < Ladder.Count)
            Ladder[s].SetCompetitor(competitor, position);
        else
            Winner = competitor;
    }
    public void LadderStageFinished()
    {
        if (FinishedLadderStages >= Ladder.Count)
            return;
        FinishedLadderStages++;
        if (FinishedLadderStages == Ladder.Count)
            State = TournamentState.Finished;
        else
        {
            LadderStage? ls = Ladder.FirstOrDefault(l => l.MatchesPlayed == 0, null);
            if(ls != null)
                ls.SetActive();
        }
            
    }
}

public enum TournamentState { Scheduled, GroupsPending, GroupsFinished, Ladder, Finished }