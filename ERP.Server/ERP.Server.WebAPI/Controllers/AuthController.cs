using ERP.Server.Application.Features.Auth.ConfirmEmail;
using ERP.Server.Application.Features.Auth.Login;
using ERP.Server.Application.Features.Auth.SendConfirmEmail;
using ERP.Server.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Server.WebAPI.Controllers;

[AllowAnonymous]
public sealed class AuthController : ApiController
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(Guid code, CancellationToken cancellationToken)
    {
        ConfirmEmailCommand request = new(code);
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> SendConfirmEmail(string email, CancellationToken cancellationToken)
    {
        SendConfirmEmailCommand request = new(email);
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
