using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SgartIT.ReactApp1.Server.DTO;
using SgartIT.ReactApp1.Server.DTO.Entities;
using SgartIT.ReactApp1.Server.DTO.Repositories;
using SgartIT.ReactApp1.Server.Repositories.MsSqlEf.DbContexts;

namespace SgartIT.ReactApp1.Server.Repositories.MsSqlEf;

public class MsSqlEfTodoRepository(ILogger<MsSqlEfTodoRepository> logger, DatabaseContext dbContext) : ITodoRepository
{
    public async Task<List<Todo>> FindAsync(string? text)
    {
        logger.LogDebug("Getting all todos with text: {text}", text);

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
        TodoEntity item;
        if (id == 0)
        {
            // Create new item
            item = new()
            {
                Title = todo.Title,
                Completed = todo.IsCompleted,
                Category = todo.Category
            };
            dbContext.Todos.Add(item);
        }
        else
        {
            // Update existing item
            item = await dbContext.Todos.FindAsync(id) ?? throw new ArgumentNullException($"Todo {id}");

            item.Title = todo.Title;
            item.Completed = todo.IsCompleted;
            item.Category = todo.Category;

        }
        //dbContext.Entry(item).State = item.Id == 0 ? EntityState.Added : EntityState.Modified;

        await dbContext.SaveChangesAsync();

        return new TodoId(default)
        {
            Id = item.Id
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
