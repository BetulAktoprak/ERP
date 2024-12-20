using ERP.Server.Application.Features.PrescriptionDetails;
using ERP.Server.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Server.WebAPI.Controllers;

public sealed class PrescriptionDetailsController : ApiController
{
    public PrescriptionDetailsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid prescriptionId, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetPrescriptionWithDetailsQuery(prescriptionId), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePrescriptionDetailCommand request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        DeletePrescriptionDetailByIdCommand request = new(id);
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}