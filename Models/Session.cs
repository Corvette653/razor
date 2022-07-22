using System.ComponentModel.DataAnnotations;

namespace razor.Models;

public class Session
{
    public string Code { get; set; } = String.Empty;
    public int UserId { get; set; } = -1;
    public DateTime ExpireDate { get; set; }
}