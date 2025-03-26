using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SgartIT.ReactApp1.Server.DTO.Entities;

namespace SgartIT.ReactApp1.Server.Repositories.Sqlite.DbContexts;

/// <summary>
/// non usare "primary constructor"
/// </summary>
/// <param name="options"></param>
public class DatabaseContext(ILogger<DbContext> logger, DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    private readonly ILogger<DbContext> logger = logger;

    public DbSet<TodoEntity> Todos { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        logger.LogTrace("Using Sqlite DbContext");

        base.OnConfiguring(optionsBuilder);
    }
}
