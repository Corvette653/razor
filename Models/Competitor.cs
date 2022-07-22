using System.ComponentModel.DataAnnotations;

namespace razor.Models;

public class Competitor
{
    public int Id { get; set; } = 0;
    [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime RegistrationDate { get; } = DateTime.Now;
    public IList<Tournament> TournamentList { get; set; } = new List<Tournament>();
    public IList<Match> MatchesList { get; set; } = new List<Match>();
    [Required]
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName
    {
        get 
        { 
            return FirstName + " " + LastName; 
        }
    }
    public bool AddTournament(Tournament newTournament)
    {
        if (TournamentList.FirstOrDefault(t => t.Id == newTournament.Id, null) != null)
            return false;
        TournamentList.Add(newTournament);
        return true;
    }
    public bool AddMatch(Match match)
    {
        if (MatchesList.FirstOrDefault(m => m.Id == match.Id, null) != null)
            return false;
        MatchesList.Add(match);
        return true;
    }
}