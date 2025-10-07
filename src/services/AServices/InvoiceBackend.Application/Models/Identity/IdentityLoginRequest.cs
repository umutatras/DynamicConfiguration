namespace InvoiceBackend.Application.Models.Identity;

public sealed class IdentityLoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public IdentityLoginRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
