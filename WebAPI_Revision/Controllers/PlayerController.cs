using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Revision.Data;
using WebAPI_Revision.DTOs.PlayerDtos;
using WebAPI_Revision.Entities;
using WebAPI_Revision.Repos_DP;
using WebAPI_Revision.Repos_DP.Interfaces;

namespace WebAPI_Revision.Controllers
{
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepos _PlayerRepos;
        private readonly ITeamRepos _TeamRepos;
        private readonly IMapper _Mapper;

        public PlayerController(IPlayerRepos PlayerRepos, ITeamRepos TeamRepos, IMapper Mapper)
        {
            _PlayerRepos = PlayerRepos;
            _Mapper = Mapper;
            _TeamRepos = TeamRepos;
        }

        #region POST Actions
        [HttpPost("AddPlayer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddPlayer([FromBody] PlayerPostDto addedPlayer)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(addedPlayer == null)
                return BadRequest();

            bool isTeamFound = await _TeamRepos.IsFound(addedPlayer.TeamId);
            if (!isTeamFound)
                return BadRequest($"No team with id: {addedPlayer.TeamId}");

            Player player = _Mapper.Map<Player>(addedPlayer);

            await _PlayerRepos.AddAsync(player);
            return Ok($"{player.Name} is added successfully to team with id: {addedPlayer.TeamId}");
        }

        [HttpPost("AddPlayers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddPlayers([FromBody] List<PlayerPostDto> addedPlayers)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            List<int> ids = addedPlayers.Select(p => p.TeamId).ToList();            
            List<int>? validIds = await _TeamRepos.GetExistingTeamIdsAsync(ids);
            List<int> invalidIds = ids.Except(validIds).ToList();

            string strInvalidIds = string.Join(",", invalidIds);

            if (invalidIds.Any())
                return BadRequest($"No Teams with Ids ({strInvalidIds})");

            List<Player> players = _Mapper.Map<List<Player>>(addedPlayers);

            await _PlayerRepos.AddRangeAsync(players);
            return Ok();
        }
        #endregion

        #region Get actions
        [HttpGet("GetPlayer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PlayerGetDto>> GetPlayer(int id)
        {
            if(id <= 0)
                return BadRequest("Invalid Player Id");

            Player? player = await _PlayerRepos.GetByIdAsync(id, false);

            if (player == null)
                return NotFound();

            if (player.IsDeleted)
                return NotFound($"{player.Name} is archived");

            PlayerGetDto playerDto = _Mapper.Map<PlayerGetDto>(player);
            return Ok(playerDto);
        }

        [HttpGet("GetPlayers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PlayerGetDto>> GetPlayers(int pageNum = 1, int pageSize = 5)
        {
            if (pageNum <= 0 || pageSize <= 0)
                return BadRequest("Invalid Inputs");

            int skipedEles = ((pageNum - 1) * pageSize);
            int takedEles = pageSize;
            List<Player>? players = await _PlayerRepos.GetAllAsync(skipedEles, takedEles, false, false);

            if (players == null || !players.Any()) // 'players.Count == 0' & '!players.Any()' check the same thing (Check empty)
                return NoContent();
            
            List<PlayerGetDto> palayersDto = _Mapper.Map<List<PlayerGetDto>>(players);

            return Ok(palayersDto);
        }

        [HttpGet("GetPlayerinDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // Returns archived player too
        public async Task<ActionResult<PlayerGetDtoInDetails>> GetPlayerinDetails(int plrId)
        {
            if (plrId <= 0)
                return BadRequest();

            Player? player = await _PlayerRepos.GetByIdAsync(plrId, false);

            if(player == null) 
                return NotFound();

            PlayerGetDtoInDetails PlrDto = _Mapper.Map<PlayerGetDtoInDetails>(player);

            return Ok(PlrDto);
        }

        [HttpGet("GetPlayersinDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<PlayerGetDtoInDetails>>> GetPlayersinDetails(int pageNum = 1, int pageSize = 5)
        {
            if (pageNum <= 0 || pageSize <= 0)
                return BadRequest();

            int skippedEles = ((pageNum - 1) * pageSize);
            int takedEles = pageSize;
            List<Player>? players = await _PlayerRepos.GetAllAsync(skippedEles, takedEles, false, false);

            if(players == null || !players.Any())
                return NoContent();

            List<PlayerGetDtoInDetails> plrsDto = _Mapper.Map<List<PlayerGetDtoInDetails>>(players);

            return Ok(plrsDto);
        }

        [HttpGet("GetArchivedPlayers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<PlayerGetDto>>> GetArchivedPlayers(int pageNum = 1, int pageSize = 5)
        {
            if(pageNum <= 0 || pageSize <= 0)
                return BadRequest();

            int skipedEles = ((pageNum - 1) * pageSize);
            int takedEles = pageSize;
            List<Player>? players = await _PlayerRepos.GetAllAsync(skipedEles, takedEles, true, false);

            if(players == null || !players.Any())
                return NoContent();

            List<PlayerGetDto> plrsDto = _Mapper.Map<List<PlayerGetDto>>(players);
            return Ok(plrsDto);
        }
        #endregion

        #region PUT Actions
        [HttpPut("UpadatePlayer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdatePlayer(int plrId, [FromBody] PlayerPostDto updatedPlr)
        {
            if (updatedPlr == null || plrId <= 0)
                return BadRequest();

            Player? player = await _PlayerRepos.GetByIdAsync(plrId, true);
            if (player is null)
                return NotFound();

            bool isTeamFound = await _TeamRepos.IsFound(updatedPlr.TeamId);
            if(!isTeamFound)
                return BadRequest("Invalid Team Id");

            _Mapper.Map(updatedPlr, player);
            player.Updatedate = DateTime.Now;

            await _PlayerRepos.UpdateAsync(player);
            return Ok($"Player {player.Id} is Updated successfully");
        }

        [HttpPut("ArchivePlayer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ArchivePlayer(int plrId)
        {
            if (plrId <= 0)
                return BadRequest("Invalid input");

            Player? player = await _PlayerRepos.GetByIdAsync(plrId, true);
            if (player is null)
                return NotFound();

            if (player.IsDeleted)
                return BadRequest($"{player.Name} is already archived");

            player.Updatedate = DateTime.Now;
            player.IsDeleted = true;
            await _PlayerRepos.UpdateAsync(player);

            return Ok($"{player.Name} is archived successfully");
        }

        [HttpPut("UnArchivePlayer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PlayerGetDto>> UnArchivePlayer(int plrId)
        {
            if (plrId <= 0)
                return BadRequest();

            Player? plr = await _PlayerRepos.GetByIdAsync(plrId, true);

            if (plr is null)
                return NotFound();

            plr.Updatedate = DateTime.Now;
            plr.IsDeleted = false;
            await _PlayerRepos.UpdateAsync(plr);

            PlayerGetDto plrDto = _Mapper.Map<PlayerGetDto>(plr);
            Response.Headers.TryAdd("Log_Msg", $"{plrDto.Name} Player is unArchived successfully");

            return Ok(plrDto);
        }
        #endregion

        #region Delete Actions
        [HttpDelete("DeletePlayer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PlayerGetDto>> DeletePlayer(int playerId)
        {
            if (playerId <= 0)
                return BadRequest();

            Player? plr = await _PlayerRepos.GetByIdAsync(playerId, true);

            if(plr is null) 
                return NotFound();

            await _PlayerRepos.DeleteAsync(plr);
            return Ok($"{plr.Name} is deleted successfully");
        }
        #endregion
    }
}