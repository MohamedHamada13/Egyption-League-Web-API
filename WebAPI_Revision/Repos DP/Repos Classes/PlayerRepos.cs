using WebAPI_Revision.Data;
using WebAPI_Revision.Entities;
using WebAPI_Revision.Repos_DP.Interfaces;

namespace WebAPI_Revision.Repos_DP.Repos_Classes
{
    public class PlayerRepos : GeneRepos<Player> , IPlayerRepos // means 'PlayerRepos' inherits from the 'GeneRepos<T>' and implement the 'IPlayerRepos'.
    {
        private readonly AppDbContext _ctx;

        public PlayerRepos(AppDbContext ctx) : base(ctx) 
        {
            _ctx = ctx;
        }
    }
}
