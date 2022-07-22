using razor.Models;

namespace razor.Services;

public static class CompetitorService
{
    static List<Competitor> Competitors { get; }
    static int nextId = 17;
    static CompetitorService()
    {
        Competitors = new List<Competitor>
        {
            new Competitor {Id = 1, FirstName = "James", LastName = "Smith"},
            new Competitor {Id = 2, FirstName = "Robert", LastName = "Johnson"},
            new Competitor {Id = 3, FirstName = "John", LastName = "Williams"},
            new Competitor {Id = 4, FirstName = "Michael", LastName = "Brown"},
            new Competitor {Id = 5, FirstName = "David", LastName = "Jones"},
            new Competitor {Id = 6, FirstName = "William", LastName = "Garcia"},
            new Competitor {Id = 7, FirstName = "Richard", LastName = "Miller"},
            new Competitor {Id = 8, FirstName = "Joseph", LastName = "Davis"},
            new Competitor {Id = 9, FirstName = "Thomas", LastName = "Rodriguez"},
            new Competitor {Id = 10, FirstName = "Charles", LastName = "Martinez"},
            new Competitor {Id = 11, FirstName = "Christopher", LastName = "Hernandez"},
            new Competitor {Id = 12, FirstName = "Daniel", LastName = "Lopez"},
            new Competitor {Id = 13, FirstName = "Matthew", LastName = "Gonzalez"},
            new Competitor {Id = 14, FirstName = "Anthony", LastName = "Wilson"},
            new Competitor {Id = 15, FirstName = "Mark", LastName = "Anderson"},
            new Competitor {Id = 16, FirstName = "Donald", LastName = "Thomas"}
        };
    }
    public static Competitor? Get(int id)
    {
        return Competitors.FirstOrDefault(c => c.Id == id, null);
    }
    public static List<Competitor>? GetAll() => Competitors;
    public static Competitor Add(string firstName, string lastName)
    {
        Competitor newCompetitor = new Competitor { 
            Id = nextId, 
            FirstName = firstName, 
            LastName = lastName 
        };
        nextId++;
        Competitors.Add(newCompetitor);
        return newCompetitor;
    }
    public static bool Remove(int id)
    {
        Competitor? competitor = Get(id);
        if (competitor == null)
            return false;
        Competitors.Remove(competitor);
        return true;
    }
}