using DynamicConfiguration.Shared.ConfigReader;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceBackend.Application.DynamicConfiguration.Queries
{
    public class GetConfigByKeyHandler : IRequestHandler<GetConfigByKeyQuery, ConfigResponse>
    {
        private readonly ConfigurationReader _reader;

        public GetConfigByKeyHandler(ConfigurationReader reader)
        {
            _reader = reader;
        }

        public Task<ConfigResponse> Handle(GetConfigByKeyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var value = _reader.GetValue<string>(request.Key);
                return Task.FromResult(new ConfigResponse
                {
                    Key = request.Key,
                    Value = value
                });
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Key '{request.Key}' bulunamadı");
            }
        }
    }
}
