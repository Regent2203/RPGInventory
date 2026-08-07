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

    [HttpPost("register")]
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

    [HttpGet("{id:guid}/profile")]
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
}