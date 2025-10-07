using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Customer.Commands.Add;

public sealed class CustomerAddCommandHandler : IRequestHandler<CustomerAddCommand, ResponseDto<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public CustomerAddCommandHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
    {
        _unitOfWork = uow;
        _currentUserService = currentUserService;
    }

    public async Task<ResponseDto<bool>> Handle(CustomerAddCommand request, CancellationToken cancellationToken)
    {
        InvoiceBackend.Domain.Entities.Customer customer = new InvoiceBackend.Domain.Entities.Customer
        {
            TaxNumber = request.TaxNumber,
            Title = request.Title,
            Address = request.Address,
            EMail = request.EMail,
            CreatedByUserId = _currentUserService.UserId,
            CreatedOn = DateTime.Now
        };
        var customerRepository = _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.Customer>();
        customerRepository.Add(customer);
        int islemSonucu = _unitOfWork.SaveChanges();
        if (islemSonucu > 0)
        {
            return new ResponseDto<bool>(true, "Customer added successfully");

        }
        return new ResponseDto<bool>(true, "Customer added failure");
    }
}
