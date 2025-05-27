using System.Diagnostics;

namespace SgartIT.ReactApp1.Server.DTO;

[DebuggerDisplay("Todo {Id}")]
public record TodoId(int Id);

[DebuggerDisplay("Todo {Id}: {Title}")]
public record TodoCreate(string Title, string Category);

[DebuggerDisplay("Todo {Id}: {Title}")]
public record TodoEdit(string Title, string Category, bool IsCompleted) : TodoCreate(Title, Category);

[DebuggerDisplay("Todo {Id}: {Title}, completed: {IsCompleted}")]
public record Todo(int Id, string Title, string Category, bool IsCompleted, DateTime Created, DateTime Modified) : TodoEdit(Title, Category, IsCompleted)
{
    //public Todo() : this(default, default, default, default, default, default) { }
}
