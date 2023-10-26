using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using BCrypt.Net;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestPipelineV5.Domain.Settings;

namespace TestPipelineV5.Domain.Auth;

[UsedImplicitly]
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IOptionsMonitor<BasicAuthenticationSettings> _basicAuthSettings;

    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        IOptionsMonitor<BasicAuthenticationSettings> basicAuthSettings, ILoggerFactory logger, UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _basicAuthSettings = basicAuthSettings;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Run an async task for validation
        return await Task.Run(() =>
        {
            // If we don't have an Authorization header, it instantly fails authorization
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization Header Missing");

            // Retrieve the token from the Authorization header
            string token;
            try
            {
                token = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]!).Parameter!;
            }
            // ReSharper disable once CatchAllClause
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            // Did the app specify a hash type other than the most secura (which is the default) of SHA512?
            var validEnum = Enum.TryParse<HashType>(_basicAuthSettings.CurrentValue.HashType, out var hashType);

            // Verify the token sent from the header with that stored in the appsettings file
            if (!BCrypt.Net.BCrypt.EnhancedVerify(token, _basicAuthSettings.CurrentValue.Token,
                    validEnum ? hashType : HashType.SHA512))
                return AuthenticateResult.Fail("Authentication Failed");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, ""),
                new Claim(ClaimTypes.Name, "")
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
        });
    }
}