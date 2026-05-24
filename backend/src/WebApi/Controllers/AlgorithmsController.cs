using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using VisualizationDSA.Application.Constants;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Application.Services;
using VisualizationDSA.Domain.Engine;
using VisualizationDSA.Domain.Input;
using VisualizationDSA.Domain.Strategies;

namespace VisualizationDSA.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AlgorithmsController : ControllerBase
{
    private readonly IEnumerable<IAlgorithmStrategy> _strategies;
    private readonly ICacheService _cacheService;

    public AlgorithmsController(IEnumerable<IAlgorithmStrategy> strategies, ICacheService cacheService)
    {
        _strategies = strategies;
        _cacheService = cacheService;
    }

    /// <summary>
    /// Lấy danh sách tất cả thuật toán có trong thư viện.
    /// GET /api/v1/algorithms
    /// </summary>
    [HttpGet]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public ActionResult<IEnumerable<object>> GetAll()
    {
        var cached = _cacheService.Get<List<object>>(CacheKeys.AlgorithmList);
        if (cached != null)
            return Ok(cached);

        var list = _strategies.Select(s => (object)new
        {
            id = s.AlgorithmId,
            name = s.Name,
            category = s.Category,
            difficulty = GetDifficulty(s.AlgorithmId),
            timeComplexity = s.GetMetadata().TimeComplexity,
            spaceComplexity = s.GetMetadata().SpaceComplexity
        }).ToList();

        _cacheService.Set(CacheKeys.AlgorithmList, list, CacheDurations.AlgorithmMetadata);
        return Ok(list);
    }

    /// <summary>
    /// Lấy siêu dữ liệu lý thuyết của thuật toán.
    /// GET /api/v1/algorithms/{algorithmId}/metadata
    /// </summary>
    [HttpGet("{algorithmId}/metadata")]
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
    public ActionResult<AlgorithmMetadata> GetMetadata(string algorithmId)
    {
        var cacheKey = $"{CacheKeys.AlgorithmMetadataPrefix}{algorithmId.ToLowerInvariant()}";
        var cached = _cacheService.Get<AlgorithmMetadata>(cacheKey);

        if (cached != null)
        {
            var etag = GenerateETag(cached);
            if (Request.Headers.IfNoneMatch.ToString() == etag)
                return StatusCode(StatusCodes.Status304NotModified);

            Response.Headers.ETag = etag;
            return Ok(cached);
        }

        var strategy = _strategies.FirstOrDefault(s =>
            s.AlgorithmId.Equals(algorithmId, StringComparison.OrdinalIgnoreCase));

        if (strategy == null)
        {
            return NotFound(new
            {
                status = 404,
                title = "Not Found",
                errorType = "ALGORITHM_NOT_FOUND",
                message = $"Không tìm thấy thuật toán tương ứng với ID: '{algorithmId}' trong thư viện hệ thống."
            });
        }

        var metadata = strategy.GetMetadata();
        _cacheService.Set(cacheKey, metadata, CacheDurations.AlgorithmMetadata);

        var etagValue = GenerateETag(metadata);
        Response.Headers.ETag = etagValue;

        return Ok(metadata);
    }

    /// <summary>
    /// Thực thi thuật toán và trả về danh sách frames hoạt họa.
    /// POST /api/v1/algorithms/execute
    /// </summary>
    [HttpPost("execute")]
    [EnableRateLimiting("execute")]
    public ActionResult<AlgorithmResult> Execute([FromBody] AlgorithmRequestDto request)
    {
        if (request.InputData.Length == 0)
        {
            return BadRequest(new
            {
                status = 400,
                title = "Bad Request",
                errorType = "EMPTY_INPUT",
                message = "Mảng dữ liệu đầu vào không được rỗng."
            });
        }

        var strategy = _strategies.FirstOrDefault(s =>
            s.AlgorithmId.Equals(request.AlgorithmId, StringComparison.OrdinalIgnoreCase));

        if (strategy == null)
        {
            return BadRequest(new
            {
                status = 400,
                title = "Bad Request",
                errorType = "UNSUPPORTED_ALGORITHM",
                message = $"Thuật toán '{request.AlgorithmId}' chưa được hỗ trợ."
            });
        }

        try
        {
            var frames = strategy.Execute(request.InputData);
            var result = new AlgorithmResult
            {
                AlgorithmId = strategy.AlgorithmId,
                PseudoCode = strategy.GetMetadata().PseudoCode,
                Frames = frames
            };
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                status = 400,
                title = "Bad Request",
                errorType = "VALIDATION_ERROR",
                message = ex.Message
            });
        }
    }

    /// <summary>
    /// Thực thi thuật toán từ chuỗi thô nhập bởi người dùng.
    /// POST /api/v1/algorithms/custom-execute
    /// </summary>
    [HttpPost("custom-execute")]
    [EnableRateLimiting("execute")]
    public async Task<ActionResult<AlgorithmResult>> CustomExecute(
        [FromBody] CustomInputRequestDto request,
        CancellationToken clientCancelToken)
    {
        int[] parsedArray;
        try
        {
            parsedArray = InputParser.ParseArray(request.RawInput);
        }
        catch (Exception ex) when (ex is ArgumentException or FormatException or OverflowException)
        {
            return BadRequest(new
            {
                status = 400,
                title = "Bad Request",
                errorType = "INVALID_FORMAT",
                message = ex.Message
            });
        }

        if (!ConstraintResolver.ValidateSize(request.AlgorithmId, parsedArray.Length, out int allowedLimit))
        {
            return UnprocessableEntity(new
            {
                status = 422,
                title = "Unprocessable Entity",
                errorType = "SIZE_LIMIT_EXCEEDED",
                message = $"Kích thước mảng vượt quá giới hạn an toàn quy định của giải thuật {request.AlgorithmId}.",
                details = new
                {
                    maxAllowedLimit = allowedLimit,
                    currentInputSize = parsedArray.Length
                }
            });
        }

        var strategy = _strategies.FirstOrDefault(s =>
            s.AlgorithmId.Equals(request.AlgorithmId, StringComparison.OrdinalIgnoreCase));

        if (strategy == null)
        {
            return BadRequest(new
            {
                status = 400,
                title = "Bad Request",
                errorType = "UNSUPPORTED_ALGORITHM",
                message = $"Thuật toán '{request.AlgorithmId}' chưa được hỗ trợ."
            });
        }

        using var timeoutSource = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        using var linkedSource = CancellationTokenSource.CreateLinkedTokenSource(
            timeoutSource.Token, clientCancelToken);

        try
        {
            var result = await Task.Run(() =>
            {
                var frames = strategy.Execute(parsedArray);
                return new AlgorithmResult
                {
                    AlgorithmId = strategy.AlgorithmId,
                    PseudoCode = strategy.GetMetadata().PseudoCode,
                    Frames = frames
                };
            }, linkedSource.Token);

            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status504GatewayTimeout, new
            {
                status = 504,
                title = "Gateway Timeout",
                errorType = "TIMEOUT_EXCEEDED",
                message = "Thời gian xử lý giải thuật vượt quá giới hạn an toàn cho phép (2 giây)."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                status = 400,
                title = "Bad Request",
                errorType = "VALIDATION_ERROR",
                message = ex.Message
            });
        }
    }

    private static string GetDifficulty(string algorithmId)
    {
        return algorithmId switch
        {
            "bubble-sort" or "selection-sort" or "insertion-sort" or "linear-search" or "stack" or "queue" => "Easy",
            "quick-sort" or "merge-sort" or "binary-search" or "bst" => "Medium",
            _ => "Medium"
        };
    }

    private static string GenerateETag(AlgorithmMetadata metadata)
    {
        var json = JsonSerializer.Serialize(metadata);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(json));
        return $"\"{Convert.ToBase64String(hash)[..16]}\"";
    }
}
