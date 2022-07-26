using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Redis.Controllers;

[ApiController]
[Route("[controller]")]
public class SqlController : ControllerBase
{
    private readonly ILogger<SqlController> _logger;
    private readonly Stopwatch _stopwatch;
    private readonly AppDbContext _appDbContext;

    public SqlController(ILogger<SqlController> logger, AppDbContext appDbContext)
    {
        _logger = logger;
        _stopwatch = new Stopwatch();
        _appDbContext = appDbContext;
    }

    [HttpGet("add/{amount}")]
    public IActionResult Add(long amount)
    {
        Seed.AddData(amount);
        _stopwatch.Start();
        _appDbContext.Presentation.AddRange(Seed.Presentations);
        _appDbContext.SaveChanges();
        _stopwatch.Stop();
        Console.WriteLine($"for {amount} data adding => Elapsed Time is {_stopwatch.ElapsedMilliseconds} ms");
        return Ok(Seed.Presentations.ToList().Take(10000));
    }

    [HttpGet("get/{amount}")]
    public IActionResult Get(long amount)
    {
        _stopwatch.Start();
        var data = _appDbContext.Presentation.OrderByDescending(x => x.Id).Take((int)amount);
        _stopwatch.Stop();
        Console.WriteLine($"for {amount} data getting => Elapsed Time is {_stopwatch.ElapsedMilliseconds} ms");
        return Ok(data.ToList().Take(10000));
    }
}
