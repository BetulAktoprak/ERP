using ERP.Server.Application.Features.Prescriptions.CreatePrescription;
using ERP.Server.Application.Features.Prescriptions.DeletePrescriptionById;
using ERP.Server.Application.Features.Prescriptions.GetAllPrescriptions;
using ERP.Server.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Server.WebAPI.Controllers;

public sealed class PrescriptionsController : ApiController
{
    public PrescriptionsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllPrescriptionsQuery(), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePrescriptionCommand request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        DeletePrescriptionByIdCommand request = new(id);
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
