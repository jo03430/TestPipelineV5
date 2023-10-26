using Microsoft.AspNetCore.Mvc;

namespace TestPipelineV5.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class UtilitiesController : BaseController
{
    /// <summary>
    /// Test API health
    /// </summary>
    [HttpGet]
    [Route("Health")]
    public IActionResult Health()
    {
        return Ok();
    }

    /// <summary>
    /// Test API unhandled exception
    /// </summary>
    /// <exception cref="Exception"></exception>
    [HttpPost, Route("Exception/Default")]
    public Task<string> TestException() => throw new Exception();

}