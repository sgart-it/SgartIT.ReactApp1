using System.Diagnostics;

namespace SgartIT.ReactApp1.Server.DTO.Entities;

[DebuggerDisplay("TodoEntity {Id}: {Title} ({Completed})")]
public class TodoEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime ModifyDate { get; set; }
}
