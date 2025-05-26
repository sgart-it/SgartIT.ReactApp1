using Microsoft.Data.SqlClient;
using SgartIT.ReactApp1.Server.DTO;
using Microsoft.Extensions.Logging;
using SgartIT.ReactApp1.Server.DTO.Repositories;
using Microsoft.Extensions.DependencyInjection;
using SgartIT.ReactApp1.Server.Repositories.MsSql.DBContexts;

namespace SgartIT.ReactApp1.Server.Repositories.MsSql;

public class MsSqlTodoRepository(ILogger<MsSqlTodoRepository> logger, [FromKeyedServices("MsSqlConnectionString")] string connectionString) : ITodoRepository
{

    public async Task<List<Todo>> FindAsync(string? text)
    {
        logger.LogDebug("Getting all todos with text: {text}", text);

        List<Todo> items = [];

        const string query = "SELECT Id, Title, Completed, Category, CreationDate, ModifyDate"
            + " FROM Todos "
            + " WHERE Title LIKE @text OR Category LIKE @text"
            + " ORDER BY Title";

        Dictionary<string, object> values = new() { { "@text", string.IsNullOrWhiteSpace(text) ? "%" : $"%{text}%" } };

        using DatabaseContext ctx = GetDbContext();
        using SqlDataReader reader = await ctx.ExecuteReaderAsync(query, values);
        while (await reader.ReadAsync())
        {
            Todo item = MapToTodo(reader);
            items.Add(item);
        }

        return items;
    }



    public async Task<Todo?> GetAsync(int id)
    {
        logger.LogDebug("Getting todo with id: {id}", id);

        const string query = "SELECT Id, Title, Completed, Category, CreationDate, ModifyDate"
            + " FROM Todos "
            + " WHERE Id = @id";

        Dictionary<string, object> values = new() { { "id", id } };

        using DatabaseContext ctx = GetDbContext();
        using SqlDataReader reader = await ctx.ExecuteReaderAsync(query, values);
        if (await reader.ReadAsync())
        {
            return MapToTodo(reader);
        }
        return null;

    }

    public async Task<TodoId> SaveAsync(int id, TodoEdit todo)
    {
        logger.LogDebug("Saving todo: {@todo}", todo);

        if (id == 0)
        {
            const string query = "INSERT INTO Todos (Title, Completed, Category, CreationDate, ModifyDate)"
               + " VALUES (@Title, @Completed, @Category, @CreationDate, @ModifyDate);"
               + " SELECT SCOPE_IDENTITY();";

            // Create new item
            using DatabaseContext ctx = GetDbContext();
            object? result = await ctx.ExecuteScalarAsync(query, new()
            {
                { "@Title", todo.Title },
                { "@Completed", todo.IsCompleted },
                { "@Category", todo.Category },
                { "@CreationDate", DateTime.UtcNow },
                { "@ModifyDate", DateTime.UtcNow }
             });
            id = Convert.ToInt32(result ?? 0);
        }
        else
        {
            const string query = "UPDATE Todos"
                + " SET Title = @title, Completed = @completed, Category = @Category, ModifyDate = @modifyDate"
                + " WHERE Id = @id";
            // Update existing item
            using DatabaseContext ctx = new(connectionString);
            await ctx.ExecuteNonQuery(query, new()
            {
                { "@Title", todo.Title },
                { "@Completed", todo.IsCompleted },
                { "@Category", todo.Category },
                { "@CreationDate", DateTime.UtcNow },
                { "@ModifyDate", DateTime.UtcNow }
             });
        }

        return new TodoId
        {
            Id = id
        };
    }


    public async Task DeleteAsync(int id)
    {
        logger.LogDebug("Deleting todo with id: {id}", id);

        const string query = "DELETE FROM Todos WHERE Id = @id";

        using DatabaseContext ctx = GetDbContext();
        await ctx.ExecuteNonQuery(query, new() { { "@id", id } });
    }


    private static Todo MapToTodo(SqlDataReader reader)
    {
        return new Todo
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1) ?? string.Empty,
            IsCompleted = reader.GetBoolean(2),
            Category = reader.GetString(3) ?? string.Empty,
            Created = reader.GetDateTime(4),
            Modified = reader.GetDateTime(5)
        };
    }

    private DatabaseContext GetDbContext()
    {
        return new (connectionString);
    }

}
