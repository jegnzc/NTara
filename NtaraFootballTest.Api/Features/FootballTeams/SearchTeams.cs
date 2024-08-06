using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NtaraFootballTest.Api.Database;
using NtaraFootballTest.Api.Entities;

namespace NtaraFootballTest.Api.Features.FootballTeams;

public class SearchTeamsRequest
{
    public required string Query { get; set; }
    public string Column { get; set; } = "All"; // Default to searching all columns
}

public class SearchTeamsResponse
{
    public required IEnumerable<FootballTeam> Teams { get; set; }
}

public class SearchTeamsValidator : Validator<SearchTeamsRequest>
{
    public SearchTeamsValidator()
    {
        RuleFor(x => x.Query).NotEmpty().WithMessage("Query cannot be empty");

        RuleFor(x => x.Column)
            .Must(column => new[] { "All", "Team", "Mascot" }.Contains(column))
            .WithMessage("Invalid column specified");
    }
}

public class SearchTeamsEndpoint(ApplicationDbContext _dbContext)
    : Endpoint<SearchTeamsRequest, SearchTeamsResponse>
{
    public override void Configure()
    {
        Get("/api/teams/search");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SearchTeamsRequest req, CancellationToken ct)
    {
        IQueryable<FootballTeam> query = _dbContext.FootballTeams;

        if (!string.IsNullOrWhiteSpace(req.Query))
        {
            query = req.Column.ToLower() switch
            {
                "team" => query.Where(t => EF.Functions.Like(t.Team, $"%{req.Query}%")),
                "mascot" => query.Where(t => EF.Functions.Like(t.Mascot, $"%{req.Query}%")),
                _
                    => query.Where(
                        t =>
                            EF.Functions.Like(t.Team, $"%{req.Query}%")
                            || EF.Functions.Like(t.Mascot, $"%{req.Query}%")
                    )
            };
        }

        var teams = await query.ToListAsync(ct);

        await SendAsync(new SearchTeamsResponse { Teams = teams }, cancellation: ct);
    }
}
