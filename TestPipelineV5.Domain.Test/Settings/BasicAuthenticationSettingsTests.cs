using Shouldly;
using TestPipelineV5.Domain.Settings;

namespace TestPipelineV5.Domain.Test.Settings;

public class BasicAuthenticationSettingsTests
{
    [Test]
    public void BasicAuthenticationSettingsTest()
    {
        var settings = new BasicAuthenticationSettings
        {
            Token = "abc",
            HashType = "def"
        };
        
        settings.Token.ShouldBe("abc");
        settings.HashType.ShouldBe("def");
    }
}