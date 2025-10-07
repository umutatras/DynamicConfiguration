namespace InvoiceBackend.Application.Customer.Queries.GetAll;

public sealed record GetAllCustomerResponse
{
    public int Id { get; set; }
    public string TaxNumber { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }
    public string EMail { get; set; }
    public GetAllCustomerResponse(int id, string taxNumber, string title, string address, string email)
    {
        Id = id;
        TaxNumber = taxNumber
            ; Title = title; Address = address; EMail = email;

    }
}
