using Microsoft.AspNetCore.Mvc;
using RPGInventory.Application.Metadata;

namespace RPGInventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetadataController : ControllerBase
{
    private readonly MetadataService _metadataService;

    public MetadataController(MetadataService metadataService)
    {
        _metadataService = metadataService;
    }

    [HttpGet]
    public async Task<ActionResult<MetadataDto>> GetGameBalance()
    {
        var config = await _metadataService.GetMetadataAsync();
        return Ok(config);
    }

    [HttpGet("AsText")]
    [Produces("text/plain")]
    public async Task<string> GetGameBalanceAsText()
    {
        var config = await _metadataService.GetMetadataAsync();
        return config.FormatedBalanceMessage;
    }
}