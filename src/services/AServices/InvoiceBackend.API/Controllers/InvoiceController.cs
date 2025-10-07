using InvoiceBackend.Application.Invoice.Commands.Add;
using InvoiceBackend.Application.Invoice.Commands.Delete;
using InvoiceBackend.Application.Invoice.Commands.Update;
using InvoiceBackend.Application.Invoice.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBackend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ApiControllerBase
{
    public InvoiceController(ISender mediatr) : base(mediatr)
    {
    }
    [HttpPost]
    public async Task<IActionResult> POST(InvoiceAddCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediatr.Send(command, cancellationToken));
    }
    [HttpPut]
    public async Task<IActionResult> PUT(InvoiceUpdateCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediatr.Send(command, cancellationToken));
    }
    [HttpDelete]
    public async Task<IActionResult> DELETE(InvoiceDeleteCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediatr.Send(command, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GET([FromQuery] GetAllInvoicesQuery query, CancellationToken cancellationToken)
    {
        return Ok(await Mediatr.Send(query, cancellationToken));
    }
}
