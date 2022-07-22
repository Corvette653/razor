using razor.Models;

namespace razor.Services;

public static class MatchService
{
    static List<Match> Matches { get; } = new List<Match>();
    static int nextId = 1;
    public static Match? Get(int id)
    {
        return Matches.FirstOrDefault(t => t.Id == id, null);
    }
    public static List<Match> GetAll() => Matches;
    public static Match Add(Match newMatch)
    {
        newMatch.Id = nextId;
        nextId++;
        Matches.Add(newMatch);
        return newMatch;
    }
}