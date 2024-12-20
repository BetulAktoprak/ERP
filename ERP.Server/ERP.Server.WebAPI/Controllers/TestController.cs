using ERP.Server.Application.Features.Mails.SendExampleMail;
using ERP.Server.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Server.WebAPI.Controllers;

public sealed class TestController : ApiController
{
    public TestController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> SendTestMail(CancellationToken cancellationToken)
    {
        SendTestMailCommand request = new();
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
