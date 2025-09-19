using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace InternshipRecords.Web.Controllers;

/// <summary>
///     Простые health-check эндпойнты для контроля состояния сервиса.
/// </summary>
[Route("api/health")]
[ApiController]
public class HealthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<HealthController> _logger;

    public HealthController(IConfiguration configuration, ILogger<HealthController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    ///     Простая проверка живости сервиса.
    ///     Возвращает 200 OK если процесс запущен.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(200)]
    public IActionResult Alive()
    {
        return Ok();
    }

    /// <summary>
    ///     Проверка готовности (readiness).
    ///     Выполняет попытку открыть соединение с базой данных и выполнить простую выборку.
    ///     Возвращает 200 OK если база доступна, 503 если недоступна.
    /// </summary>
    /// <param name="ct">Token отмены запроса.</param>
    /// <returns>200 OK или 503 Service Unavailable.</returns>
    [HttpPost("ready")]
    [ProducesResponseType(200)]
    [ProducesResponseType(503)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Ready(CancellationToken ct)
    {
        try
        {
            var connString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connString))
            {
                _logger.LogWarning("Connection string 'DefaultConnection' not found for readiness check.");
                return StatusCode(500, "Database connection string not configured.");
            }

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1";
            await cmd.ExecuteScalarAsync(ct);

            return Ok();
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Readiness check cancelled.");
            return StatusCode(503);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database readiness check failed.");
            return StatusCode(503);
        }
    }
}