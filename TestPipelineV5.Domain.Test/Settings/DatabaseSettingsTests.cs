using Shouldly;
using TestPipelineV5.Domain.Settings;

namespace TestPipelineV5.Domain.Test.Settings;

public class DatabaseSettingsTests
{
    [Test]
    public void DatabaseConnectionTest() => new DatabaseSettings
    {
        DatabaseConnection1 = "abc"
    }.DatabaseConnection1.ShouldBe("abc");
}