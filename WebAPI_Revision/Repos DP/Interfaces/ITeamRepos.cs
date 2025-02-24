using WebAPI_Revision.DTOs.TeamDtos;
using WebAPI_Revision.Entities;
using WebAPI_Revision.Repos_DP.Interfaces;

namespace WebAPI_Revision.Repos_DP
{
    public interface ITeamRepos : IGeneRepos<Team>
    {
        public Task<Team?> GetTeamWithPlayersAsync(int teamId);
        public Task<List<int>?> GetExistingTeamIdsAsync(List<int> ids);
    }
}
