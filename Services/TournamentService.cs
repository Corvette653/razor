using razor.Models;

namespace razor.Services;

public static class TournamentService
{
    static List<Tournament> Tournaments { get; } = new List<Tournament>();
    static int nextId = 5;
    static TournamentService() 
    {
        Tournaments = new List<Tournament> {
        new Tournament{ Id = 1, Name = "ladder tournament"},
        new Tournament{ Id = 2, Name = "group tournament", GroupStage = true, NumberOfPlayersInGroup = 5, NumberOfGroupWinners = 2},
        new Tournament{ Id = 3, Name = "incomplete ladder tournament"},
        new Tournament{ Id = 4, Name = "incomplete group tournament", GroupStage = true, NumberOfPlayersInGroup = 3, NumberOfGroupWinners = 1}
        };
        for(int i = 1; i < 9; i++)
            Tournaments[0].AddCompetitor(CompetitorService.Get(i));
        for (int i = 1; i < 11; i++)
            Tournaments[1].AddCompetitor(CompetitorService.Get(i));
        for (int i = 4; i < 11; i++)
            Tournaments[2].AddCompetitor(CompetitorService.Get(i));
        for (int i = 7; i < 15; i++)
            Tournaments[3].AddCompetitor(CompetitorService.Get(i));
        Tournaments[0].CreateLadder();
        Tournaments[1].CreateGroups();
        Tournaments[2].CreateLadder();
        Tournaments[3].CreateGroups();
        List<Match> matches = MatchService.GetAll();
        Random rand = new Random();
        foreach(Match match in matches)
        {
            if (match.Played)
                continue;
            if (rand.Next(2) == 0) // returns 0 or 1
                match.SetWinner(match.FirstCompetitor);
            else
                match.SetWinner(match.SecondCompetitor);
        }
        Tournaments[1].CreateLadder();
        Tournaments[3].CreateLadder();
        matches = MatchService.GetAll();
        foreach(Match match in matches)
        {
            if (match.Played)
                continue;
            if (rand.Next(2) == 0) // returns 0 or 1
                match.SetWinner(match.FirstCompetitor);
            else
                match.SetWinner(match.SecondCompetitor);
        }
    }
    public static Tournament? Get(string name)
    {
        return Tournaments.FirstOrDefault(t => t.Name == name, null);
    }
    public static Tournament? Get(int id)
    {
        return Tournaments.FirstOrDefault(t => t.Id == id, null);
    }
    public static List<Tournament> GetAll() => Tournaments;
    public static Tournament? Add(Tournament newTournament)
    {
        if (Get(newTournament.Name) != null)
            return null;
        newTournament.Id = nextId;
        nextId++;
        Tournaments.Add(newTournament);
        return newTournament;
    }
}