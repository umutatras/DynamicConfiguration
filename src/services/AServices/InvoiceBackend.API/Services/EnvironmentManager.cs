using InvoiceBackend.Application.Interfaces;

namespace InvoiceBackend.API.Services
{
    public sealed class EnvironmentManager : IEnvironmentService
    {
        public string WebRootPath { get; }
        public EnvironmentManager(string webRootPath)
        {
            WebRootPath = webRootPath;
        }
    }
}
