using Microsoft.EntityFrameworkCore;
using WebAPI_Revision.Data;
using WebAPI_Revision.Entities;
using WebAPI_Revision.Repos_DP.Repos_Classes;
using WebAPI_Revision.DTOs.PlayerDtos;
using WebAPI_Revision.DTOs.TeamDtos;

namespace WebAPI_Revision.Repos_DP
{
    public class TeamRepos : GeneRepos<Team>, ITeamRepos // Means 'TeamRepos' inherits from 'GeneRepos<Team>', and implement ITeamRepos
    {
        private readonly AppDbContext _ctx;
        public TeamRepos(AppDbContext ctx) : base(ctx) 
        {
            _ctx = ctx;
        }

        public async Task<Team?> GetTeamWithPlayersAsync(int teamId)
        {
            Team? team = await _ctx.Teams
                .AsNoTracking()
                .Include(t => t.Players) // Eager Loading
                .FirstOrDefaultAsync(t => t.Id == teamId);

            return team;
        }

        public async Task<List<int>?> GetExistingTeamIdsAsync(List<int> ids) => 
            await _ctx.Teams
                .Where(t => ids.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync();
    }
}
