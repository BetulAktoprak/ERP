using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Server.WebAPI.Abstractions;
[Route("api/[controller]/[action]")]
[ApiController]
public abstract class ApiController : ControllerBase
{
    public readonly IMediator mediator;

    public ApiController(IMediator mediator)
    {
        this.mediator = mediator;
    }
}
