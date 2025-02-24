using Microsoft.AspNetCore.Http;
using WebAPI_Revision.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Revision.Migrations;
using WebAPI_Revision.AutoMapperMappingProfiles;
using AutoMapper;
using WebAPI_Revision.CustomLogging;
using Microsoft.EntityFrameworkCore;
using WebAPI_Revision.Repos_DP;
using WebAPI_Revision.DTOs.TeamDtos;

namespace WebAPI_Revision.Controllers
{
    [Route("api/[controller]")] // -> [Rouet("api/Team")]

    // [ApiController] /// We have to cancel this anntotation to can use the 'ModelState', and this enforce us to write [FromBody] in Post action para by ourself
    public class TeamController : ControllerBase
    {
        // Objects Injection 
        private readonly IMapper _mapper;
        private readonly ILogger<TeamController> _logger;
        private readonly ICustomLog _cusLog;                // CustomLog _cusLog = new CustomLog();
        private readonly ITeamRepos _TeamRepos;

        #region Notes for DI Pattern
        /// Dependancy means the 'TeamController' depends on 'AppDbContext', 'Mapper', 'Logger', and 'CustomLog' classes in his work, and Injection means you create an interface for each class and inject these classes interface in the constructor of the 'TeamController' instead of using classes directly, as when you use the 'TeamController' like this TeamController TC = new TeamController(..,..,..);, The interfaces will call the classes that implments it, You can not understand the moral of these steps, but Later when you trying to scale the project by adding a new class instead of old class you just need to craete the new class and make it implements the interface of the old class, and in the start_up file change the regestration to the new class, then you discover that scaling the project become very easy, as without DI Pattern you need to edit in all project controllers.
        #endregion

        public TeamController(IMapper mapper, ILogger<TeamController> logger, ICustomLog cusLog, ITeamRepos TeamRepos)
        {
            _mapper = mapper;
            _logger = logger;
            _cusLog = cusLog;
            _TeamRepos = TeamRepos;
        }


        // Search about the changeTracker
        // Apply Onion structure

        #region Repository DP
        // We was deal with Database directly with '_ctx' object, But with applying Repos DP we deals with repos using '_TeamRepos' and repos deal with database.
        #endregion

        #region Notes for Response header collection & ActionResult Response
        /// - Write the message in the return value like -> "return BadRequest("Invalid Id");", if you want to show it to the client (front-end, Mobile App), or API Consumers, While Logging is more better for system concerns messages or If the error contains sensitive data that the client should not Look at her(security).
        #endregion

        #region Post Actions 
        [HttpPost("AddTeam")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> AddTeam([FromBody] TeamPostDto newTeam)
        {
            if (!ModelState.IsValid) // 'ModelState' here referes to TeamPostDto DTO model, So we have to apply the validation in DTOs.
                return BadRequest(ModelState);

            Team team = _mapper.Map<Team>(newTeam);
            // Team 'AddedDate' Prop value is setted automatically from the Team Class, and 'UpdatedDate' property setted as 'null'.

            await _TeamRepos.AddAsync(team);
            return Ok();
        }

        [HttpPost("AddTeams")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddTeams([FromBody] List<TeamPostDto> newTeams) // Bulk Insert
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (newTeams == null || newTeams.Count == 0 || !newTeams.Any())
                return BadRequest();

            List<Team> teams = _mapper.Map<List<Team>>(newTeams);

            await _TeamRepos.AddRangeAsync(teams);
            return Ok();
        }
        #endregion 

        #region Get Actions 
        [HttpGet("GetTeams")] 
        [ProducesResponseType(StatusCodes.Status200OK)] // Document the expected responses
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<TeamGetDto>>> GetTeams(int pageNum = 1, int pageSize = 5) 
        {
            if(pageSize <= 0 ||  pageNum <= 0)
            {
                _logger.Log(LogLevel.Warning, "Invalid Inputs");
                return BadRequest();
            }

            int skipedEles = (pageNum - 1) * pageSize;
            int TakedEles = pageSize;

            List<Team>? currPageTeams = await _TeamRepos
                .GetAllAsync(skipedEles, TakedEles, false, false);

            if (currPageTeams == null || currPageTeams.Count == 0)
            {
                _logger.Log(LogLevel.Warning , "There are not teams.");
                return NoContent();
            }
             
            List<TeamGetDto> teamsDtos = new();
            _mapper.Map(currPageTeams, teamsDtos);

            Response.Headers.TryAdd("Note", "only unArchived teams are returned");
            return Ok(teamsDtos);
        }

        [HttpGet("GetArchivedTeams")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<TeamGetDto>>> GetArchivedTeams(int pageNum = 1, int pageSize = 5)
        {
            if(pageSize <= 0 || pageNum <= 0)
            {
                _logger.Log(LogLevel.Warning, "Invalid Inputs");
                return BadRequest();
            }

            int skipedEles = ((pageNum - 1) * pageSize);
            int takedEles = pageSize;

            List<Team>? archivedTeams = await _TeamRepos
                .GetAllAsync(skipedEles, takedEles, true, false);

            if(archivedTeams == null || archivedTeams.Count == 0)
            {
                Response.Headers.TryAdd("Note", "There are not archived teams.");
                _logger.Log(LogLevel.Warning, "There are not archived teams.");
                _cusLog.AddLog("There are not archived teams.");
                return NoContent();
            }
                
            List<TeamGetDto> archivedTeamsDto = new();
            _mapper.Map(archivedTeams, archivedTeamsDto);

            Response.Headers.TryAdd("Note", "Only Archived Teams are Returned");
            return Ok(archivedTeamsDto);
        }

        [HttpGet("GetTeam")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TeamGetDto>> GetTeam(int id)
        { 
            if (id <= 0)
                return BadRequest("Invalid Id"); // Use Logging Instead of writting messages in the return value.

            var team = await _TeamRepos
                .GetByIdAsync(id, false);

            if (team is null)
                return NoContent();

            if (team.IsDeleted)
                return NotFound($"{team.Name} team is archived");

            TeamGetDto teamDto = _mapper.Map<TeamGetDto>(team);

            return Ok(teamDto);
        }

        [HttpGet("GetTeamWithPlayers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<TeamWithPlayersGetDto>> GetTeamWithPlayers(int teamId)
        {
            if (teamId <= 0)
                return BadRequest("Invalid team id");

            Team? team = await _TeamRepos // Load Data
                .GetTeamWithPlayersAsync(teamId);
                
            if (team is null)
                return NoContent();
                
            if (team.IsDeleted)
                return NotFound($"{team.Name} team is archived");

            TeamWithPlayersGetDto teamDto = _mapper.Map<TeamWithPlayersGetDto>(team);
            // Important Note: The mapping here is double, where 'Team' class is mapped into 'TeamWithPlayersGetDto' and 'List<Player>' property is mapped into 'List<PlayerGetDto>'.
            return Ok(teamDto);
        }
        #endregion

        #region PUT Actions
        [HttpPut("UpdateTeam")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateTeam(int teamId, [FromBody] TeamUpdateDto updatedTeam)
        {
            if(updatedTeam == null || teamId <= 0)
            {
                Response.Headers.TryAdd("Note", "Invalid Team Id");
                return BadRequest("Invalid Inputs");
            }
                
            Team? team = await _TeamRepos
                .GetByIdAsync(teamId, true); // Do not use AsNoTracking(), as you will tracke the object by updating it.

            if (team is null)
            {
                Response.Headers.TryAdd("Note", $"No team with id {teamId}");
                _logger.Log(LogLevel.Warning, $"No team with id {teamId}");
                return NoContent();
            }

            _mapper.Map(updatedTeam, team);
            team.UpdatedDate = DateTime.Now;

            await _TeamRepos.UpdateAsync(team);
            return Ok($"{team?.Name} Team is updated successfully");
        }

        [HttpPut("ArchiveTeam")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ArchiveTeam(int teamId) // Soft Delete
        {
            if (teamId <= 0)
                return BadRequest("Invalid Id");

            Team? team = await _TeamRepos
                .GetByIdAsync(teamId, true);

            if (team is null)
                return NotFound($"No team with id = {teamId}");

            if (team.IsDeleted)
                return BadRequest($"{team.Name} team already archived");

            team.UpdatedDate = DateTime.Now;
            team.IsDeleted = true;

            await _TeamRepos.UpdateAsync(team);

            return Ok($"{team.Name} team is Archived successfully");
        }
        
        [HttpPut("UnArchiveTeam")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TeamGetDto>> UnArchiveTeam(int teamId)
        {
            if (teamId <= 0)
                return BadRequest("Invalid id");

            Team? team = await _TeamRepos
                .GetByIdAsync(teamId, true);

            if (team is null)
                return NotFound($"No team with id = {teamId}");

            if (!team.IsDeleted)
                return BadRequest($"{team.Name} is actually exist (has unArchived)");

            team.IsDeleted = false; // UnArchive Team

            TeamGetDto teamDto = _mapper.Map<TeamGetDto>(team);

            await _TeamRepos.UpdateAsync(team);
            Response.Headers.TryAdd("Log_Msg", $"{teamDto.Name} team is unArchived successfully");
            return Ok(teamDto);

            // Imp Note: In Response.Headers Collection, It's not valid to use spaces in the header key.
        }
        #endregion

        #region Delete Actions
        [HttpDelete("DeleteTeam")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTeam(int teamId) // Hard Delete
        {
            if (teamId <= 0)
                return BadRequest("Invalid id");

            Team? team = await _TeamRepos 
                .GetByIdAsync(teamId, true);

            if (team is null) 
                return NotFound($"No Team with id = {teamId}");

            await _TeamRepos.DeleteAsync(team); // Remove & SaveChanges

            return Ok($"'{team.Name}' team has been deleted");
        }
        
        
        // [HttpDelete("DeleteTeamsHard")] // Hard Bulk Delete
        // [HttpDelete("DeleteTeamsSoft")] // Soft Bulk Delete
        #endregion
    }
}
