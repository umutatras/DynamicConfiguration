using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Customer.Commands.Delete;

public sealed class CustomerDeleteCommandHandler : IRequestHandler<CustomerDeleteCommand, ResponseDto<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public CustomerDeleteCommandHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
    {
        _unitOfWork = uow;
        _currentUserService = currentUserService;
    }

    public async Task<ResponseDto<bool>> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
    {

        var customerRepository = _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.Customer>().GetByFilter(filter: x => x.Id == request.Id, asNoTracking: true);
        if (customerRepository is null)
        {
            return new ResponseDto<bool>(false, "Customer not found");
        }

        _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.Customer>().Remove(customerRepository);
        int islemSonucu = _unitOfWork.SaveChanges();
        if (islemSonucu > 0)
        {
            return new ResponseDto<bool>(true, "Customer Delete successfully");

        }
        return new ResponseDto<bool>(true, "Customer Delete failure");
    }
}
