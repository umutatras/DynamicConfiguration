using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Invoice.Commands.Update;

public sealed class InvoiceUpdateCommandHandler : IRequestHandler<InvoiceUpdateCommand, ResponseDto<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public InvoiceUpdateCommandHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
    {
        _unitOfWork = uow;
        _currentUserService = currentUserService;
    }

    public async Task<ResponseDto<bool>> Handle(InvoiceUpdateCommand request, CancellationToken cancellationToken)
    {

        var invoiceRepository = _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.Invoice>().Find(request.Id);
        if (invoiceRepository is null)
        {
            return new ResponseDto<bool>(false, "Invoice not found");
        }
        invoiceRepository.CustomerId = request.CustomerId;
        invoiceRepository.InvoiceDate = request.InvoiceDate;
        invoiceRepository.TotalAmount = request.TotalAmount;
        invoiceRepository.InvoiceNumber = request.InvoiceNumber;
        invoiceRepository.ModifiedByUserId = _currentUserService.UserId;
        invoiceRepository.ModifiedOn = DateTime.Now;
        invoiceRepository.InvoiceLines = request.InvoiceLines.Select(s => new Domain.Entities.InvoiceLine { ItemName = s.ItemName, Price = s.Price, Quentity = s.Quentity }).ToList();
        int islemSonucu = _unitOfWork.SaveChanges();

        if (islemSonucu > 0)
        {
            return new ResponseDto<bool>(true, "Invoice added successfully");

        }
        return new ResponseDto<bool>(true, "Invoice added failure");
    }
}