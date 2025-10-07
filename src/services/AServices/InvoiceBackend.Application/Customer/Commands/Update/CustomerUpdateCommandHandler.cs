using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Customer.Commands.Update;

public sealed class CustomerUpdateCommandHandler : IRequestHandler<CustomerUpdateCommand, ResponseDto<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public CustomerUpdateCommandHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
    {
        _unitOfWork = uow;
        _currentUserService = currentUserService;
    }

    public async Task<ResponseDto<bool>> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
    {

        var customerRepository = _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.Customer>().GetByFilter(filter: x => x.Id == request.Id, asNoTracking: true);
        if (customerRepository is null)
        {
            return new ResponseDto<bool>(false, "Customer not found");
        }
        customerRepository.TaxNumber = request.TaxNumber;
        customerRepository.Title = request.Title; customerRepository.Address = request.Address; customerRepository.EMail = request.EMail; customerRepository.ModifiedByUserId = _currentUserService.UserId; customerRepository.ModifiedOn = DateTime.Now;
        _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.Customer>().Update(customerRepository);
        int islemSonucu = _unitOfWork.SaveChanges();
        if (islemSonucu > 0)
        {
            return new ResponseDto<bool>(true, "Customer update successfully");

        }
        return new ResponseDto<bool>(true, "Customer update failure");
    }
}
