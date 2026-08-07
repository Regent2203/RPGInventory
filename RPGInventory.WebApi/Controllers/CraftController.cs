using Microsoft.AspNetCore.Mvc;
using RPGInventory.Application.Crafting;

namespace RPGInventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CraftController : ControllerBase
{
    private readonly CraftService _craftService;

    public CraftController(CraftService craftService)
    {
        _craftService = craftService;
    }

    [HttpPost]
    public async Task<ActionResult<CraftItemResponse>> Craft([FromBody] CraftItemRequest request)
    {
        try
        {
            var result = await _craftService.CraftAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { error = ex.Message });
        }
    }
}