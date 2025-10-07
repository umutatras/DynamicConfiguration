using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Invoice.Queries.GetAll;

public class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQuery, ResponseDto<List<GetAllInvoicesResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllInvoicesQueryHandler(IUnitOfWork uow)
    {
        _unitOfWork = uow;
    }

    public async Task<ResponseDto<List<GetAllInvoicesResponse>>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        var result = _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.Invoice>().Query().Where(s => s.CreatedOn >= request.StartDate && s.CreatedOn <= request.EndDate).Select(s =>
        new GetAllInvoicesResponse
        {
            Id = s.Id,
            CustomerId = s.CustomerId,
            InvoiceDate = s.InvoiceDate,
            InvoiceNumber = s.InvoiceNumber,
            TotalAmount = s.TotalAmount,
            InvoiceLines = s.InvoiceLines.Select(s => new Models.Invoice.InvoiceUpdateRequestDto { ItemName = s.ItemName, Price = s.Price, Quentity = s.Quentity }).ToList()
        }).ToList();

        return new ResponseDto<List<GetAllInvoicesResponse>>
        { Data = result, Success = true, Message = string.Empty, Errors = null };
    }
}