using System.ComponentModel.DataAnnotations;
using razor.Services;
namespace razor.Models;

public class Group
{
    public int Id { get; set; }
    public char Name { get; set; }
    public bool MatchesCreated { get; set; } = false;
    public int MatchesPlayed { get; set; } = 0;
    public Tournament Tournament { get; set; } = new Tournament();
    public IList<CompPoint> Competitors { get; set; } = new List<CompPoint>(); // remember to sort after changing
    public IList<Match> Matches { get; set; } = new List<Match>();
    public void SortGroup()
    {
        List<CompPoint> sortedComp = new List<CompPoint>(Competitors);
        sortedComp.Sort(delegate (CompPoint x, CompPoint y)
        {
            if (x.Points > y.Points)
                return -1;
            return 1;
        });
        Competitors = sortedComp;
    }
    public bool CreateMatches()
    {
        if (MatchesCreated)
            return false;
        MatchesCreated = true;
        for(int i = 0; i < Competitors.Count; i++)
        {
            for(int j = i+1; j < Competitors.Count; j++)
            {
                Match m = new Match { Group = this };
                m.SetFirstCompetitor(Competitors[i].Competitor);
                m.SetSecondCompetitor(Competitors[j].Competitor);
                m = MatchService.Add(m);
                Matches.Add(m);
            }
        }
        MixMatches();
        return true;
    }
    public void MixMatches()
    {
        Random random = new Random();
        int n = Matches.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Match m = Matches[k];
            Matches[k] = Matches[n];
            Matches[n] = m;
        }
    }
    public void MatchPlayed()
    {
        if (MatchesPlayed >= Matches.Count)
            return;
        MatchesPlayed++;
        if (MatchesPlayed == Matches.Count)
            Tournament.GroupFinished();
    }
    public bool AddPoints(Competitor competitor, int points)
    {
        //CompPoint? x = Competitors.FirstOrDefault(c => c.Competitor.Id == competitor.Id, none); // doesn't work - because of struct? 
        for (int i = 0; i < Competitors.Count; i++) // workaround to find specific element
        {
            if(Competitors[i].Competitor.Id == competitor.Id)
            {
                CompPoint x = Competitors[i]; // competitors[i] returns a copy of an object so there is a need to create a new instance and replace the old one
                x.Points += points;
                Competitors[i] = x;
            }
        }
        return false;
    }
}

public struct CompPoint
{
    public Competitor Competitor { get; set; }
    public int Points { get; set; }
    public CompPoint(Competitor c, int p)
    {
        Competitor = c;
        Points = p;
    }
}