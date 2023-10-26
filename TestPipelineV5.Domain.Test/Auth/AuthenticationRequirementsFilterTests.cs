using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Shouldly;
using TestPipelineV5.Domain.Auth;

namespace TestPipelineV5.Domain.Test.Auth;

public class AuthenticationRequirementsFilterTests
{
    [Test]
    public void AuthenticationRequirementsFilterTest()
    {
        var filter = new AuthenticationRequirementsFilter();
        var op = new OpenApiOperation();
        
        filter.Apply(op, null!);
        
        op.Security.Count.ShouldBe(1);
        
        var scheme = op.Security[0];
        scheme.Keys.Count.ShouldBe(1);
        
        var keys = scheme.Keys.ToList();
        keys[0].Reference.Type.ShouldBe(ReferenceType.SecurityScheme);
        keys[0].Reference.Id.ShouldBe("Bearer");
        
        scheme[keys[0]].ShouldBeEmpty();
    }
}