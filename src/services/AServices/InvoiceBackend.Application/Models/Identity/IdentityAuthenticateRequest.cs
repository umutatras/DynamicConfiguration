namespace InvoiceBackend.Application.Models.Identity;

public sealed class IdentityAuthenticateRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public IdentityAuthenticateRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
