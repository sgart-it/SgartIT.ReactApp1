namespace SgartIT.ReactApp1.Server.DTO.Repositories;

public interface ITodoRepository
{
    Task<List<Todo>> FindAsync(string? text);

    Task<Todo?> GetAsync(int id);

    /// <summary>
    /// id==0 insert 
    /// id!=0 update
    /// </summary>
    /// <param name="id"></param>
    /// <param name="todo"></param>
    /// <returns></returns>
    Task<TodoId> SaveAsync(int id, TodoEdit todo);

    Task DeleteAsync(int id);
}
