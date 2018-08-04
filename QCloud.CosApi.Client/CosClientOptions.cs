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

        public CosClientOptions Value
        {
            get
            {
                if (string.IsNullOrEmpty(SecretId))
                    throw new ArgumentNullException(nameof(SecretId));
                if (string.IsNullOrEmpty(SecretId))
                    throw new ArgumentNullException(nameof(SecretKey));
                if (AppId <= 0)
                    throw new ArgumentOutOfRangeException(nameof(AppId));

                return this;
            }
        }
    }
}
