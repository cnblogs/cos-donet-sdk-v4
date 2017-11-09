using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace QCloud.CosApi.Client
{
    public class CosClientOptions : IOptions<CosClientOptions>
    {
        public int AppId { get; set; }

        public string SecretId { get; set; }

        public string SecretKey { get; set; }

        public int HttpTimeout { get; set; } = 60;

        public CosClientOptions Value => this;
    }
}
