using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected readonly ISender Mediatr;

        public ApiControllerBase(ISender mediatr)
        {
            Mediatr = mediatr;
        }
    }
}
