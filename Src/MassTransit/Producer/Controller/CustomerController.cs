using Common.Model;
using MassTransit;
using Microsoft.AspNetCore.Mvc;


namespace Producer.Controller;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{

    private readonly IPublishEndpoint _publish;
    public CustomerController(IPublishEndpoint publish)
    {
        _publish = publish;
    }

    [HttpGet("publish")]
    public async Task<IActionResult> Get(int Id)
    {
        await _publish.Publish<Customer>(new
        {
            Id = 1,
            FirstName = "test"+Guid.NewGuid().ToString(),
            LastName = "Test",
            age = 20
        });
        return Ok();
    }

}
