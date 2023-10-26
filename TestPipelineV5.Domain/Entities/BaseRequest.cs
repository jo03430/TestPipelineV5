using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace TestPipelineV5.Domain.Entities;

[UsedImplicitly]
public abstract class BaseRequest
{
    /// <summary>
    /// Unique Id to each request, defaulted if not included in request
    /// </summary>
    [UsedImplicitly]
    [FromHeader(Name = "X-Correlation-Id")] 
    public string? CorrelationId { get; set; } = Guid.NewGuid().ToString();
}
