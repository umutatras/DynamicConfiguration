using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceBackend.Application.DynamicConfiguration.Queries
{
    public class GetConfigByKeyQuery : IRequest<ConfigResponse>
    {
        public string Key { get; set; }

        public GetConfigByKeyQuery(string key)
        {
            Key = key;
        }
    }

    public class ConfigResponse
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
