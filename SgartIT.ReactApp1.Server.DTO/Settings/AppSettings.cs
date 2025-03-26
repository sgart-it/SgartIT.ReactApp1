using System.ComponentModel.DataAnnotations;

namespace SgartIT.ReactApp1.Server.DTO.Settings;

public class AppSettings
{
    public const string KEY_NAME = "AppSettings";

    [Required]
    public required string ExampleProperty { get; set; }

    //public RepositorySettings Repository { get; set; } = new();

    public string[] Cors { get; set; } = [];
}
