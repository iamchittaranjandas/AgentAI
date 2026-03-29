using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Retrieval;
using AgentAI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgentAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RetrievalController : ControllerBase
{
    private readonly IRetrievalService _retrievalService;
    private readonly ILogger<RetrievalController> _logger;

    public RetrievalController(
        IRetrievalService retrievalService,
        ILogger<RetrievalController> logger)
    {
        _retrievalService = retrievalService;
        _logger = logger;
    }

    [HttpPost("search")]
    public async Task<ActionResult<ApiResponse<RetrievalResultDto>>> Search([FromBody] RetrievalRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing retrieval search request");

        var result = await _retrievalService.SearchAsync(request, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new ApiResponse<RetrievalResultDto>
            {
                Success = false,
                Errors = result.Errors,
                Message = result.Message
            });
        }

        return Ok(new ApiResponse<RetrievalResultDto>
        {
            Success = true,
            Data = result.Data,
            Message = result.Message
        });
    }
}
