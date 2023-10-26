using System;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using TestPipelineV5.Controllers.V1;

namespace TestPipelineV5.Test.Controllers;

public class UtilitiesControllerTests
{
    private UtilitiesController _utilitiesController = null!;

    [SetUp]
    public void SetUp() => _utilitiesController = new UtilitiesController();
    
    [Test]
    public void HealthTest() => _utilitiesController.Health().ShouldBeOfType(typeof(OkResult));

    [Test]
    public void TestException() => Should.Throw<Exception>(_utilitiesController.TestException);
}