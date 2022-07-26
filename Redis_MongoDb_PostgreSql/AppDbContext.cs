using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Presentation> Presentation { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}

public class Presentation
{
    public long? Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
}
