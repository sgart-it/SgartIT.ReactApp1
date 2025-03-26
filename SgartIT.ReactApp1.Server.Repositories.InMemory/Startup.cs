using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SgartIT.ReactApp1.Server.DTO.Repositories;
using SgartIT.ReactApp1.Server.Repositories.InMemory.DBContexts;

namespace SgartIT.ReactApp1.Server.Repositories.InMemory;

public static class Startup
{
    public static void Init(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DatabaseContext>();
        builder.Services.AddScoped<ITodoRepository, InMemoryTodoRepository>();
    }
}
