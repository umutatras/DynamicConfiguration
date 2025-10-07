using InvoiceBackend.API;
using InvoiceBackend.API.Filters;
using InvoiceBackend.Infrastructure;
using InvoiceBackend.Persistence;
using InvoiceBackend.Application;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
});
// Add services to the container.
builder.Services.AddApplicaton();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddWebApi(builder.Configuration, builder.Environment);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
