using System.Text.Encodings.Web;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using TestPipelineV5.Domain.Auth;
using TestPipelineV5.Domain.Settings;

namespace TestPipelineV5.Domain.Test.Auth;

public class BasicAuthenticationHandlerTests
{
    private BasicAuthenticationHandler _handler;
    private IOptionsMonitor<BasicAuthenticationSettings> _basicAuthOptions;

    [SetUp]
    public void SetUp()
    {
        var options = Substitute.For<IOptionsMonitor<AuthenticationSchemeOptions>>();
        options.Get(Arg.Any<string>()).Returns(new AuthenticationSchemeOptions());

        _basicAuthOptions = Substitute.For<IOptionsMonitor<BasicAuthenticationSettings>>();
        
        
        var logger = Substitute.For<ILogger<BasicAuthenticationHandler>>();
        var loggerFactory = Substitute.For<ILoggerFactory>();
        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(logger);

        var encoder = Substitute.For<UrlEncoder>();
        var clock = Substitute.For<ISystemClock>();

        _handler = new BasicAuthenticationHandler(options, _basicAuthOptions, loggerFactory, encoder, clock);
    }
    
    [Test]
    public async Task BasicAuthenticationHandlerMissingAuthorizationHeaderTest()
    {

        var context = new DefaultHttpContext();

        await _handler.InitializeAsync(new AuthenticationScheme("", null, typeof(BasicAuthenticationHandler)), context);
        var result = await _handler.AuthenticateAsync();
        
        result.Failure.ShouldNotBeNull().Message.ShouldBe("Authorization Header Missing");
    }
    
    [Test]
    public async Task BasicAuthenticationHandlerAuthorizationHeaderEmptyValueTest()
    {

        var context = new DefaultHttpContext();
        context.Request.Headers.Add("Authorization", string.Empty);

        await _handler.InitializeAsync(new AuthenticationScheme("", null, typeof(BasicAuthenticationHandler)), context);
        var result = await _handler.AuthenticateAsync();
        
        result.Failure.ShouldNotBeNull().Message.ShouldBe("Invalid Authorization Header");
    }
    
    [Test]
    public async Task BasicAuthenticationHandlerAuthorizationHeaderWithValueInvalidHashTypeTest()
    {
        _basicAuthOptions.CurrentValue.Returns(new BasicAuthenticationSettings
        {
            HashType = "",
            Token = BCrypt.Net.BCrypt.EnhancedHashPassword("Test", HashType.SHA512)
        });
        
        var context = new DefaultHttpContext();
        context.Request.Headers.Add("Authorization", "Bearer Test");

        await _handler.InitializeAsync(new AuthenticationScheme("", null, typeof(BasicAuthenticationHandler)), context);
        var result = await _handler.AuthenticateAsync();
        
        result.Failure.ShouldBeNull();
        result.Succeeded.ShouldBeTrue();
    }
    
    [Test]
    public async Task BasicAuthenticationHandlerAuthorizationHeaderWithValueValidHashTypeTest()
    {
        _basicAuthOptions.CurrentValue.Returns(new BasicAuthenticationSettings
        {
            HashType = "SHA384",
            Token = BCrypt.Net.BCrypt.EnhancedHashPassword("Test", HashType.SHA384)
        });
        
        var context = new DefaultHttpContext();
        context.Request.Headers.Add("Authorization", "Bearer Test");

        await _handler.InitializeAsync(new AuthenticationScheme("", null, typeof(BasicAuthenticationHandler)), context);
        var result = await _handler.AuthenticateAsync();
        
        result.Failure.ShouldBeNull();
        result.Succeeded.ShouldBeTrue();
    }
    
    [Test]
    public async Task BasicAuthenticationHandlerAuthorizationHeaderWithValueIncorrectTokenTest()
    {
        _basicAuthOptions.CurrentValue.Returns(new BasicAuthenticationSettings
        {
            HashType = "SHA384",
            Token = BCrypt.Net.BCrypt.EnhancedHashPassword("Test2", HashType.SHA384)
        });
        
        var context = new DefaultHttpContext();
        context.Request.Headers.Add("Authorization", "Bearer Test");

        await _handler.InitializeAsync(new AuthenticationScheme("", null, typeof(BasicAuthenticationHandler)), context);
        var result = await _handler.AuthenticateAsync();
        
        result.Failure.ShouldNotBeNull().Message.ShouldBe("Authentication Failed");
    }
}