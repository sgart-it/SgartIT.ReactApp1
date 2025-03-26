namespace SgartIT.ReactApp1.Server.DTO.Repositories;

public interface ITodoRepository
{
    Task<List<Todo>> FindAsync(string? text);

    Task<Todo?> GetAsync(int id);

    Task<TodoId> SaveAsync(int id, TodoEdit todo);

    Task DeleteAsync(int id);
}
