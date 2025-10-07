using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Application.Models.General;
using MediatR;

namespace InvoiceBackend.Application.Auth.Commands.Login;

public class AuthLoginCommandHandler : IRequestHandler<AuthLoginCommand, ResponseDto<AuthLoginDto>>
{
    private readonly IIdentityService _identityService;

    public AuthLoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ResponseDto<AuthLoginDto>> Handle(AuthLoginCommand request, CancellationToken cancellationToken)
    {
        var response = await _identityService.LoginAsync(request.ToIdentityLoginRequest());
        return new ResponseDto<AuthLoginDto>(AuthLoginDto.FromIdentityLoginResponse(response));
    }


}
