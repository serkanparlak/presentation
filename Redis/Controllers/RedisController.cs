using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Redis.Controllers;

[ApiController]
[Route("[controller]")]
public class RedisController : ControllerBase
{
    private readonly ILogger<SqlController> _logger;
    private readonly Stopwatch _stopwatch;

    public RedisController(ILogger<SqlController> logger, AppDbContext appDbContext)
    {
        _logger = logger;
        _stopwatch = new Stopwatch();
    }

    [HttpGet("add/{amount}")]
    public IActionResult Add(long amount)
    {
        var presentations = Seed.SeedPresentations(amount);
        _stopwatch.Start();
        RedisCacheManager.Set<IList<Presentation>>($"presentation-{amount}", presentations);
        _stopwatch.Stop();
        Console.WriteLine($"for {amount} data adding => Elapsed Time is {_stopwatch.ElapsedMilliseconds} ms");
        return Ok(presentations.ToList().Take(100));
    }

    [HttpGet("get/{amount}")]
    public IActionResult Get(long amount)
    {
        _stopwatch.Start();
        var data = RedisCacheManager.Get<IList<Presentation>>($"presentation-{amount}");
        _stopwatch.Stop();
        Console.WriteLine($"for {amount} data getting => Elapsed Time is {_stopwatch.ElapsedMilliseconds} ms");
        return Ok(data.ToList().Take(100));
    }
}
