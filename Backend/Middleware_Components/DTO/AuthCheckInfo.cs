using Middleware_Components.JWT.DTO.CheckUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO
{
    public class AuthCheckInfo
    {
        public Auth_CheckSuccess? check_success { get; set; }

        public Auth_CheckError? check_error { get; set; }

        public bool CheckHasError() { return check_error != null; }

        public bool CheckHasSuccess() { return check_success != null; }
    }
}
