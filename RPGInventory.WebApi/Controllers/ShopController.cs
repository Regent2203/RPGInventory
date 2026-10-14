using Microsoft.AspNetCore.Mvc;
using RPGInventory.Application.Shop;

namespace RPGInventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopController : ControllerBase
{
    private readonly ShopService _shopService;

    public ShopController(ShopService shopService)
    {
        _shopService = shopService;
    }

    [HttpPost("Buy")]
    public async Task<ActionResult<TradeResponse>> Buy([FromBody] TradeRequest request)
    {
        try
        {
            var result = await _shopService.BuyItemAsync(request);
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

    [HttpPost("Sell")]
    public async Task<ActionResult<TradeResponse>> Sell([FromBody] TradeRequest request)
    {
        try
        {
            var result = await _shopService.SellItemAsync(request);
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