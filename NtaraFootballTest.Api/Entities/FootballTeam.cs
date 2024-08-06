namespace NtaraFootballTest.Api.Entities;

public class FootballTeam
{
    public int Id { get; set; }
    public required string Team { get; set; }
    public required string Mascot { get; set; }
    public DateOnly? DateOfLastWin { get; set; }
    public double? WinningPercentage { get; set; }
    public int? Wins { get; set; }
    public int? Losses { get; set; }
    public int? Ties { get; set; }
    public int? Games { get; set; }
}
