using InvoiceBackend.Application.DynamicConfiguration.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicConfigurationController : ApiControllerBase
    {
        public DynamicConfigurationController(ISender mediatr) : base(mediatr)
        {
        }
        [HttpGet("{key}")]
        public async Task<IActionResult> GetConfigValue(string key)
        {
            try
            {
                var result = await Mediatr.Send(new GetConfigByKeyQuery(key));
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Key = key, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
