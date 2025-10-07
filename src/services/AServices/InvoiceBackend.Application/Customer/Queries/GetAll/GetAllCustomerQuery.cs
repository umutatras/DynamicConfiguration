using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Customer.Queries.GetAll;

public sealed class GetAllCustomerQuery : IRequest<ResponseDto<List<GetAllCustomerResponse>>>
{

}