namespace InvoiceBackend.Application.Models.Jwt
{
    public class JwtGenerateTokenRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public JwtGenerateTokenRequest(int id, string email, IList<string> roles)
        {
            Id = id;
            Email = email;
            Roles = roles;
        }
    }
}
