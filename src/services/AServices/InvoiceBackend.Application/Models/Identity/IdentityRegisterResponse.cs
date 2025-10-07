namespace InvoiceBackend.Application.Models.Identity;

public class IdentityRegisterResponse
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string EmailToken { get; set; }
    public IdentityRegisterResponse(int id, string email, string emailToken)
    {
        Id = id;
        Email = email;
        EmailToken = emailToken;
    }
}
