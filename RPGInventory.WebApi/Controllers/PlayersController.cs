using Microsoft.AspNetCore.Mvc;
using RPGInventory.Application.Players;

namespace RPGInventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly PlayerService _playerService;

    public PlayersController(PlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<RegisterPlayerResponse>> Register([FromBody] RegisterPlayerRequest request)
    {
        try
        {
            var result = await _playerService.RegisterAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}/Profile")]
    public async Task<ActionResult<PlayerProfileDto>> GetProfile(Guid id)
    {
        try
        {
            var profile = await _playerService.GetProfileAsync(id);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}/Inventory")]
    public async Task<ActionResult<PlayerProfileDto>> GetInventory(Guid id)
    {
        try
        {
            var profile = await _playerService.GetInventoryAsync(id);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}/Inventory/AsText")]
    [Produces("text/plain")]
    public async Task<ActionResult<PlayerProfileDto>> GetInventoryAsText(Guid id)
    {
        try
        {
            var profile = await _playerService.GetInventoryAsTextAsync(id);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}/History")]
    public async Task<ActionResult<List<InventoryHistoryDto>>> GetHistory(Guid id)
    {
        try
        {
            var history = await _playerService.GetHistoryAsync(id);
            return Ok(history);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}