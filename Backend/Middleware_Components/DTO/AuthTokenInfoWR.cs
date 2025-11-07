using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO
{
    public class AuthTokenInfoWR
    {
        public string accessToken { get; set; }

        public string refreshToken { get; set; }

        public DateTime expires_at { get; set; }
    }
}
