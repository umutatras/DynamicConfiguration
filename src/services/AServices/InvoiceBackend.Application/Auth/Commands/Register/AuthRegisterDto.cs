using InvoiceBackend.Application.Models.Identity;

namespace InvoiceBackend.Application.Auth.Commands.Register;

public class AuthRegisterDto
{
    public int UserId { get; set; }
    public string EmailToken { get; set; }


    public AuthRegisterDto(int userId, string emailToken)
    {
        UserId = userId;
        EmailToken = emailToken;
    }

    public static AuthRegisterDto Create(IdentityRegisterResponse response)
    {
        return new AuthRegisterDto(response.Id, response.EmailToken);
    }
}
