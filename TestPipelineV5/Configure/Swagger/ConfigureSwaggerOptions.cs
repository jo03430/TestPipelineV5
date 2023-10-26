using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TestPipelineV5.Domain.Auth;

namespace TestPipelineV5.Configure.Swagger
{
    public class ConfigureSwaggerOptions
        : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Configure each API discovered for Swagger Documentation
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            // add swagger document for every API version discovered
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
                
            }
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Description = "Input your API Token",
                In = ParameterLocation.Header,
                Name = "Authorization"
            });
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TestPipelineV5.xml"));
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TestPipelineV5.Domain.xml"));
            options.OperationFilter<AuthenticationRequirementsFilter>();
        }

        /// <summary>
        /// Configure Swagger Options. Inherited from the Interface
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        /// <summary>
        /// Create information about the version of the API
        /// </summary>
        /// <param name="desc"></param>
        /// <returns>Information about the API</returns>
        private OpenApiInfo CreateVersionInfo(
            ApiVersionDescription desc)
        {
            var info = new OpenApiInfo()
            {
                Title = "TestPipelineV5",
                Version = desc.ApiVersion.ToString("'v'V"),
                Description = "[API Description]",
                Contact = new OpenApiContact
                {
                    Name = "[Responsible Group Name]",
                    Email = "[Group Email Address]",
                    Url = new Uri("https://securainsurance.sharepoint.com")
                }
            };

            if (desc.IsDeprecated)
            {
                info.Description +=
                    " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
            }

            return info;
        }
    }
}
