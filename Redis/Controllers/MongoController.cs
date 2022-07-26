using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Redis.Controllers;

[ApiController]
[Route("[controller]")]
public class MongoController : ControllerBase
{
    private readonly ILogger<MongoController> _logger;
    private readonly Stopwatch _stopwatch;
    private readonly MongoDBService _mongoDBService;

    public MongoController(ILogger<MongoController> logger, MongoDBService mongoDBService)
    {
        _logger = logger;
        _stopwatch = new Stopwatch();
        _mongoDBService = mongoDBService;
    }

    [HttpGet("add/{amount}")]
    public async Task<IActionResult> Add(long amount)
    {
        var playlist = Seed.SeedPlaylist(amount);
        _stopwatch.Start();
        await _mongoDBService.AddBulk(playlist);
        _stopwatch.Stop();
        // log
        _logger.LogInformation($"for {amount} data adding => Elapsed Time is {_stopwatch.ElapsedMilliseconds} ms");
        return Ok(playlist.ToList().Take(100));
    }

    [HttpGet("get/{amount}")]
    public async Task<IActionResult> Get(long amount)
    {
        _stopwatch.Start();
        var data = await _mongoDBService.GetAllAsync();
        _stopwatch.Stop();
        // log
        _logger.LogInformation($"for {amount} data getting => Elapsed Time is {_stopwatch.ElapsedMilliseconds} ms");
        return Ok(data.ToList().Take(100));
    }
}
