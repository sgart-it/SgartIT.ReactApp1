using Microsoft.Extensions.Options;
using SgartIT.ReactApp1.Server.DTO;
using SgartIT.ReactApp1.Server.DTO.Repositories;
using SgartIT.ReactApp1.Server.DTO.Settings;
using SgartIT.ReactApp1.Server.Exports;

namespace SgartIT.ReactApp1.Server.Services;

public class MainService(ILogger<MainService> logger, ITodoRepository repository, IOptions<AppSettings> iOptAppSettings)
{
    readonly AppSettings appSettings = iOptAppSettings.Value;

    public async Task<List<Todo>> FindAsync(string? text)
    {
        logger.LogDebug("Finding todos with text {text}, ExampleProperty: {example}", text, appSettings.ExampleProperty);

        return await repository.FindAsync(text);
    }

    public async Task<Todo?> GetAsync(int id)
    {
        logger.LogDebug("Getting todo with id {Id}", id);

        return await repository.GetAsync(id);
    }

    public async Task<TodoId> CreateAsync(TodoCreate todo)
    {
        logger.LogDebug("Creating todo with title {Title}", todo.Title);

        TodoEdit todoE = new()
        {
            Title = todo.Title,
            Category = todo.Category,
            IsCompleted = false
        };

        return await repository.SaveAsync(0, todoE);
    }

    public async Task UpdateAsync(int id, TodoEdit todo)
    {
        logger.LogDebug("Updating todo with id {id}", id);

        await repository.SaveAsync(id, todo);
    }

    public async Task DeleteAsync(int id)
    {
        logger.LogDebug("Deleting todo with id {Id}", id);

        await repository.DeleteAsync(id);
    }

}
