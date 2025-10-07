using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Customer.Queries.GetAll;

public sealed class GetAllCustomerQueryHandler : IRequestHandler<GetAllCustomerQuery, ResponseDto<List<GetAllCustomerResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllCustomerQueryHandler(IUnitOfWork uow)
    {
        _unitOfWork = uow;
    }

    public async Task<ResponseDto<List<GetAllCustomerResponse>>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
    {
        var result = _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.Customer>().Query().Select(s => new GetAllCustomerResponse(s.Id, s.TaxNumber, s.Title, s.Address, s.EMail)).ToList();
        return new ResponseDto<List<GetAllCustomerResponse>> { Data = result, Success = true, Message = string.Empty, Errors = null };
    }
}