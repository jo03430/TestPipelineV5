using Shouldly;
using TestPipelineV5.Domain.Entities;

namespace TestPipelineV5.Domain.Test.Entities;

public class BaseRequestTests
{
    public class TestCase : BaseRequest
    {
        
    }

    [Test]
    public void BaseRequestTest() => new TestCase {CorrelationId = "abc"}.CorrelationId.ShouldBe("abc");
}