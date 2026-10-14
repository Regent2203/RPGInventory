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
        var metadata = await _metadataService.GetMetadataAsync();
        return Ok(metadata);
    }

    [HttpGet("AsText")]
    [Produces("text/plain")]
    public async Task<string> GetGameBalanceAsText()
    {
        var text = await _metadataService.GetMetadataAsTextAsync();
        return text;
    }
}