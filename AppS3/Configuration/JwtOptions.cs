using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.Configuration
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }

        public string Audience { get; set; }

        public string Issuer { get; set; }

        public int ExpireInSecond { get; set; }
    }
}
