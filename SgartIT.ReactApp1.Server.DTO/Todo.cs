using System.Diagnostics;

namespace SgartIT.ReactApp1.Server.DTO;


[DebuggerDisplay("Todo {Id}")]
public class TodoId
{
    public int Id { get; set; }
}

[DebuggerDisplay("Todo {Id}: {Title}")]
public class TodoCreate
{
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

[DebuggerDisplay("Todo {Id}: {Title}")]
public class TodoEdit : TodoCreate
{
    public bool IsCompleted { get; set; }
}

[DebuggerDisplay("Todo {Id}: {Title}")]
public class Todo : TodoEdit
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}