using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Invoice.Commands.Delete;

public sealed class InvoiceDeleteCommandHandler : IRequestHandler<InvoiceDeleteCommand, ResponseDto<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    public InvoiceDeleteCommandHandler(IUnitOfWork uow)
    {
        _unitOfWork = uow;
    }

    public async Task<ResponseDto<bool>> Handle(InvoiceDeleteCommand request, CancellationToken cancellationToken)
    {

        var invoiceRepository = _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.Invoice>().Find(request.Id);
        if (invoiceRepository is null)
        {
            return new ResponseDto<bool>(false, "Invoice not found");
        }
        _unitOfWork.GetRepository<InvoiceBackend.Domain.Entities.Invoice>().Remove(invoiceRepository);
        int islemSonucu = _unitOfWork.SaveChanges();

        if (islemSonucu > 0)
        {
            return new ResponseDto<bool>(true, "Invoice delete successfully");

        }
        return new ResponseDto<bool>(true, "Invoice delete failure");
    }
}