using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Auth.Commands.Register;

public class AuthRegisterCommandHandler : IRequestHandler<AuthRegisterCommand, ResponseDto<AuthRegisterDto>>
{
    private readonly IIdentityService _identityService;

    public AuthRegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ResponseDto<AuthRegisterDto>> Handle(AuthRegisterCommand request, CancellationToken cancellationToken)
    {
        var response = await _identityService.RegisterAsync(request.ToIdentityRegisterRequest());
        return new ResponseDto<AuthRegisterDto>(AuthRegisterDto.Create(response), "User Register Successfuly");
    }
}
