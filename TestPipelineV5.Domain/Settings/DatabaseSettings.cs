using JetBrains.Annotations;

namespace TestPipelineV5.Domain.Settings;

[PublicAPI]
public class DatabaseSettings
{
    public string? DatabaseConnection1 { get; set; }
}