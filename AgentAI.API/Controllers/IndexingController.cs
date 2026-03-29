using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Admin;
using AgentAI.Application.DTOs.Indexing;
using AgentAI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgentAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IndexingController : ControllerBase
{
    private readonly IIndexingService _indexingService;
    private readonly ILogger<IndexingController> _logger;

    public IndexingController(
        IIndexingService indexingService,
        ILogger<IndexingController> logger)
    {
        _indexingService = indexingService;
        _logger = logger;
    }

    [HttpPost("start")]
    public async Task<ActionResult<ApiResponse<IndexingStatusDto>>> StartIndexing([FromBody] IndexingRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting indexing for repository {RepositoryPath}", request.RepositoryPath);

        var result = await _indexingService.StartIndexingAsync(request, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new ApiResponse<IndexingStatusDto>
            {
                Success = false,
                Errors = result.Errors,
                Message = result.Message
            });
        }

        return Ok(new ApiResponse<IndexingStatusDto>
        {
            Success = true,
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpGet("status/{jobId}")]
    public async Task<ActionResult<ApiResponse<IndexingStatusDto>>> GetStatus(Guid jobId, CancellationToken cancellationToken)
    {
        var result = await _indexingService.GetIndexingStatusAsync(jobId, cancellationToken);

        if (!result.Success)
        {
            return NotFound(new ApiResponse<IndexingStatusDto>
            {
                Success = false,
                Errors = result.Errors,
                Message = result.Message
            });
        }

        return Ok(new ApiResponse<IndexingStatusDto>
        {
            Success = true,
            Data = result.Data
        });
    }

    [HttpGet("repositories")]
    public async Task<ActionResult<ApiResponse<List<RepositoryDto>>>> GetRepositories(CancellationToken cancellationToken)
    {
        var result = await _indexingService.GetIndexedRepositoriesAsync(cancellationToken);

        return Ok(new ApiResponse<List<RepositoryDto>>
        {
            Success = true,
            Data = result.Data
        });
    }
}
