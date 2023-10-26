using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Secura.Infrastructure.Autofac;
using Secura.Infrastructure.Logging.NLog;
using TestPipelineV5.Domain;
using TestPipelineV5.Domain.Auth;
using TestPipelineV5.Domain.Settings;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using NLogLoggerFactory = Secura.Infrastructure.Logging.NLog.NLogLoggerFactory;
using TestPipelineV5.Configure.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Secura.Infrastructure.Core.WebApi.Extensions.HttpLogging;
using Secura.Infrastructure.Core.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment;
IWebHostBuilder webHostBuilder = builder.WebHost;

webHostBuilder.ConfigureLogging(x =>
{
    x.ClearProviders();
    x.SetMinimumLevel(LogLevel.Trace);
}).UseNLog();

#region Configuration Setup

var environmentConfiguration = new ConfigurationBuilder()
    .SetBasePath(environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
    .AddEnvironmentVariables();

IConfiguration configuration = environmentConfiguration.Build();

#endregion


#region Configure Nlog

LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));

#endregion


builder.Services.AddControllers();

#region Configure Api Versioning

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.Conventions.Add(new VersionByNamespaceConvention());
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"));
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

#endregion

#region Configure Swagger

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

#endregion


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<LoggingModule<NLogLoggerFactory, NLogRetrieveLogs>>();
    containerBuilder.RegisterModule<TestPipelineV5DomainAutofacRegistration>();
});

#region Configure Settings Classes

builder.Services.Configure<ErrorHandlingSettings>(configuration.GetSection("ErrorHandling"));
builder.Services.Configure<DatabaseSettings>(configuration.GetSection("Database"));

builder.Services.Configure<BasicAuthenticationSettings>(configuration.GetSection("BasicAuthentication"));
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

#endregion

#region Configure HttpClients

//Configure list of headers to add to propagation
//NOTE: Must still configure [HttpClient]s to 'AddHeaderPropagation'
//  Example: builder.Services.AddHttpClient("Client").AddHeaderPropagation
builder.Services.AddHeaderPropagation(options => { options.Headers.Add("X-Correlation-Id"); });

//Add [HttpClient]s for [IHttpClientFactory]
//  Example:    builder.Services.AddHttpClient("MyClient", client =>
//              {
//                  client.BaseAddress = new Uri("[MyBaseAddress]");
//              }).AddHeaderPropagation();

#endregion

#region Configure HttpLogging/RequestResponseLogging

//Secura extension that uses Secura's HttpLoggingSettings class to configure HttpLoggingOptions based on appsettings
builder.Services.ConfigureHttpLogging(configuration);

#endregion

#region Build Application

var app = builder.Build();

//Secura extension to create request delegate that sets the 'X-Correlation-Id' header if not present (GUID)
app.UseCorrelationDelegate();

//Example setting HttpLogging for particular endpoints
//  Only paths starting with utilities, targeting utilities controller
//app.UseHttpLoggingForEndpointsStartingWith(endpointStartingSegments: new[] { "/utilities" });

app.UseAuthentication();

#region Build Swagger UI

app.UseSwagger();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
    options.DocExpansion(DocExpansion.List);
    // build a Swagger endpoint for each discovered API version
    foreach (var description in provider.ApiVersionDescriptions.Reverse())
    {
        options.SwaggerEndpoint($"{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }
    options.DocumentTitle = environment.IsProduction() ? "TestPipelineV5 API" : $"{environment.EnvironmentName} - TestPipelineV5 API";
});

#endregion


app.UseMiddleware<UnhandledExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHeaderPropagation();

app.MapControllers();

#endregion

app.Run();



#pragma warning disable CA1050
[ExcludeFromCodeCoverage]
[UsedImplicitly]
public partial class Program { }
#pragma warning restore CA1050
