using SgartIT.ReactApp1.Server.DTO;
using Microsoft.Extensions.Logging;
using SgartIT.ReactApp1.Server.DTO.Repositories;
using SgartIT.ReactApp1.Server.Repositories.Sqlite.DbContexts;
using Microsoft.EntityFrameworkCore;
using SgartIT.ReactApp1.Server.DTO.Entities;

namespace SgartIT.ReactApp1.Server.Repositories.Sqlite;

public class SqliteTodoRepository(ILogger<SqliteTodoRepository> logger, DatabaseContext dbContext) : ITodoRepository
{
    private static bool _databaseCreated = false;

    public async Task<List<Todo>> FindAsync(string? text)
    {
        logger.LogDebug("Getting all todos with text: {text}", text);

        if (_databaseCreated == false)
        {
            await dbContext.Database.EnsureCreatedAsync();
            _databaseCreated = true;
        }
        string textLower = string.IsNullOrWhiteSpace(text) ? string.Empty : text.ToLower();

        List<Todo> items = await dbContext.Todos
            .AsNoTracking()
            .Where(item => textLower == string.Empty
                || item.Title.Contains(textLower, StringComparison.CurrentCultureIgnoreCase)
                || item.Category.Contains(textLower, StringComparison.CurrentCultureIgnoreCase))
            .Select(item => MapToTodo(item))
            .ToListAsync();

        return items;
    }

    public async Task<Todo?> GetAsync(int id)
    {
        logger.LogDebug("Getting todo with id: {id}", id);

        var item = await dbContext.Todos.FindAsync(id);

        return item == null ? null : MapToTodo(item);
    }

    public async Task<TodoId> SaveAsync(int id, TodoEdit todo)
    {
        logger.LogDebug("Saving todo: {@todo}", todo);

        if (id == 0)
        {
            // Create new item
            TodoEntity item = new()
            {
                Title = todo.Title,
                Completed = todo.IsCompleted,
                Category = todo.Category,
                CreationDate = DateTime.UtcNow,
                ModifyDate = DateTime.UtcNow
            };
            dbContext.Todos.Add(item);
            await dbContext.SaveChangesAsync();
            id = item.Id;
        }
        else
        {
            // Update existing item
            var item = await dbContext.Todos.FindAsync(id) ?? throw new ArgumentNullException($"Todo {id}");

            item.Title = todo.Title;
            item.Completed = todo.IsCompleted;
            item.Category = todo.Category;
            item.ModifyDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
        }

        return new TodoId(default)
        {
            Id = id
        };
    }

    public async Task DeleteAsync(int id)
    {
        logger.LogDebug("Deleting todo with id: {id}", id);

        var item = await dbContext.Todos.FindAsync(id) ?? throw new ArgumentNullException($"Todo {id}");

        dbContext.Remove(item);

        await dbContext.SaveChangesAsync();
    }

    private static Todo MapToTodo(TodoEntity item) => new(item.Id, item.Title, item.Category, item.Completed, item.CreationDate, item.ModifyDate);

}
