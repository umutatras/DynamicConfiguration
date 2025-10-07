using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Invoice.Commands.Add;

public sealed class InvoiceAddCommandHandler : IRequestHandler<InvoiceAddCommand, ResponseDto<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public InvoiceAddCommandHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
    {
        _unitOfWork = uow;
        _currentUserService = currentUserService;
    }

    public async Task<ResponseDto<bool>> Handle(InvoiceAddCommand request, CancellationToken cancellationToken)
    {
        InvoiceBackend.Domain.Entities.Invoice invoice = new InvoiceBackend.Domain.Entities.Invoice
        {
            CustomerId = request.CustomerId,
            InvoiceDate = request.InvoiceDate,
            TotalAmount = request.TotalAmount,
            InvoiceNumber = request.InvoiceNumber,
            CreatedByUserId = _currentUserService.UserId,
            CreatedOn = DateTime.Now,
            InvoiceLines = request.InvoiceLines.Select(s => new Domain.Entities.InvoiceLine { ItemName = s.ItemName, Price = s.Price, Quentity = s.Quentity }).ToList(),
        };
        var invoiceRepository = _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.Invoice>();
        invoiceRepository.Add(invoice);
        int islemSonucu = _unitOfWork.SaveChanges();

        if (islemSonucu > 0)
        {
            return new ResponseDto<bool>(true, "Invoice added successfully");

        }
        return new ResponseDto<bool>(true, "Invoice added failure");
    }
}
