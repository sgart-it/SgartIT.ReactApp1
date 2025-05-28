using SgartIT.ReactApp1.Server.DTO;
using Microsoft.Extensions.Logging;
using SgartIT.ReactApp1.Server.DTO.Repositories;
using SgartIT.ReactApp1.Server.Repositories.MsSqlDapper.DBContexts;

namespace SgartIT.ReactApp1.Server.Repositories.MsSqlDapper;

public class MsSqlDapperTodoRepository(ILogger<MsSqlDapperTodoRepository> logger, DatabaseContext dbContext) : ITodoRepository
{

    public async Task<List<Todo>> FindAsync(string? text)
    {
        logger.LogDebug("Getting all todos with text: {text}", text);

        //  ERROR A parameterless default constructor or one matching signature
        // ATTENZIONE: l'ordine dei campi, ed il nome, è importante per la serializzazione in JSON,
        // per evitare di avere un ordine casuale
        // e per evitare di dover usare [JsonPropertyName] su ogni campo della classe Todo
        const string query = """
            SELECT Id, Title, Category, Completed AS IsCompleted, CreationDate AS Created, ModifyDate AS Modified
            FROM Todos
            WHERE Title LIKE @Text OR Category LIKE @Text
            ORDER BY Title
            """;

        var parameters = new
        {
            Text = string.IsNullOrWhiteSpace(text) ? "%" : $"%{text}%"
        };

        return [.. await dbContext.QueryAsync<Todo>(query, parameters)];
    }


    public async Task<Todo?> GetAsync(int id)
    {
        logger.LogDebug("Getting todo with id: {id}", id);

        const string query = """
            SELECT Id, Title, Category, Completed AS IsCompleted, CreationDate AS Created, ModifyDate AS Modified
            FROM Todos
            WHERE Id = @Id
            """;

        return await dbContext.QueryFirstOrDefaultAsync<Todo>(query, new { id });
    }

    public async Task<TodoId> SaveAsync(int id, TodoEdit todo)
    {
        logger.LogDebug("Saving todo: {@todo} with id: {id}", todo, id);

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
                todo.Title, // fa match con il nome della porprietà
                Completed = todo.IsCompleted,   // in questo caso i nomi non coincidono
                todo.Category,
                CreationDate = DateTime.UtcNow
            };
            id = await dbContext.QueryFirstAsync<int>(query, parameters);
        }
        else
        {
            const string query = """
                UPDATE Todos SET
                    Title = @Title, 
                    Completed = @IsCompleted, 
                    Category = @Category, 
                    ModifyDate = @ModifyDate
                WHERE Id = @Id
                """;
            // Update existing item
            var parameters = new
            {
                Id = id,
                todo.Title,
                todo.IsCompleted,
                todo.Category,
                ModifyDate = DateTime.UtcNow
            };
            await dbContext.QueryAsync(query, parameters);
        }
        return new TodoId(id);
    }


    public async Task DeleteAsync(int id)
    {
        logger.LogDebug("Deleting todo with id: {id}", id);

        const string query = "DELETE FROM Todos WHERE Id = @Id";

        await dbContext.QueryAsync<Todo>(query, new { id });
    }

}
