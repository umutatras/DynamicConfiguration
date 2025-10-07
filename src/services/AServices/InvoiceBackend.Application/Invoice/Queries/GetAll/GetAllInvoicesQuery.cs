using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Invoice.Queries.GetAll;

public class GetAllInvoicesQuery : IRequest<ResponseDto<List<GetAllInvoicesResponse>>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
