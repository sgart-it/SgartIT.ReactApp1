using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SgartIT.ReactApp1.Server.DTO.Entities;

namespace SgartIT.ReactApp1.Server.Repositories.InMemory.DBContexts;

public class DatabaseContext(ILogger<DatabaseContext> logger) : DbContext
{
    public DbSet<TodoEntity> Todos { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        logger.LogTrace("Using InMemoryDbContext");

        optionsBuilder.UseInMemoryDatabase("ReactAppDb");

        base.OnConfiguring(optionsBuilder);
    }
}
