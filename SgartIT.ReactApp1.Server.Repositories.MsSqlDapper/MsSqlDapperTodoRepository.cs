using Microsoft.Data.SqlClient;
using SgartIT.ReactApp1.Server.DTO;
using Microsoft.Extensions.Logging;
using SgartIT.ReactApp1.Server.DTO.Repositories;
using Microsoft.Extensions.DependencyInjection;
using SgartIT.ReactApp1.Server.Repositories.MsSqlDapper.Helpers;
using Dapper;

namespace SgartIT.ReactApp1.Server.Repositories.MsSqlDapper;

public class MsSqlDapperTodoRepository(ILogger<MsSqlDapperTodoRepository> logger, [FromKeyedServices("MsSqlConnectionString")] string connectionString) : ITodoRepository
{

    public async Task<List<Todo>> FindAsync(string? text)
    {
        logger.LogDebug("Getting all todos with text: {text}", text);

        const string query = """
            SELECT Id, Title, Completed, Category, CreationDate AS Created, ModifyDate AS Modified
            FROM Todos
            WHERE Title LIKE @text OR Category LIKE @Text
            ORDER BY Title
            """;

        var parameters = new
        {
            Text = string.IsNullOrWhiteSpace(text) ? "%" : $"%{text}%"
        };

        using SqlConnection cnn = Helper.GetConnection(connectionString);
        return [.. await cnn.QueryAsync<Todo>(query, parameters)];
    }


    public async Task<Todo?> GetAsync(int id)
    {
        logger.LogDebug("Getting todo with id: {id}", id);

        const string query = """
            SELECT Id, Title, Completed, Category, CreationDate AS Created, ModifyDate AS Modified
            FROM Todos
            WHERE Id = @Id
            """;

        var parameters = new
        {
            Id = id
        };

        using SqlConnection cnn = Helper.GetConnection(connectionString);
        return await cnn.QueryFirstAsync<Todo>(query, parameters);
    }

    public async Task<TodoId> SaveAsync(int id, TodoEdit todo)
    {
        logger.LogDebug("Saving todo: {@todo}", todo);

        if (id == 0)
        {
            const string query = """
                INSERT INTO Todos (Title, Completed, Category, CreationDate, ModifyDate)
                VALUES (@Title, @Completed, @Category, @CreationDate, @CreationDate);
                SELECT SCOPE_IDENTITY();
                """;
            // insert new item
            var parameters = new
            {
                todo.Title,
                Completed = todo.IsCompleted,
                todo.Category,
                CreationDate = DateTime.UtcNow
            };

            using SqlConnection cnn = Helper.GetConnection(connectionString);
            id = await cnn.QuerySingleAsync<int>(query, parameters);
        }
        else
        {
            const string query = """
                UPDATE Todos SET
                Title = @Title, Completed = @Completed, Category = @Category, ModifyDate = @ModifyDate
                WHERE Id = @Id
                """;
            // Update existing item
            var parameters = new
            {
                Id = id,
                todo.Title,
                Completed = todo.IsCompleted,
                todo.Category,
                ModifyDate = DateTime.UtcNow
            };

            using SqlConnection cnn = Helper.GetConnection(connectionString);
            await cnn.QueryAsync(query, parameters);
        }

        return new TodoId
        {
            Id = id
        };
    }


    public async Task DeleteAsync(int id)
    {
        logger.LogDebug("Deleting todo with id: {id}", id);

        const string query = "DELETE FROM Todos WHERE Id = @Id";

        var parameters = new
        {
            Id = id
        };

        using SqlConnection cnn = Helper.GetConnection(connectionString);
        await cnn.QueryAsync(query, parameters);
    }

}
