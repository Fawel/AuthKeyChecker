using KeyChecker.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeyChecker.Application.Models
{
    public class ApplicationCodeAuthKeyValidateRequest
    {
        public readonly ApplicationCode RequestingApplicationCode;
        public readonly ApplicationCode TargetApplicationCode;
        public readonly AuthKeyValue Key;

        public ApplicationCodeAuthKeyValidateRequest(
            ApplicationCode requestingApplicationCode,
            ApplicationCode targetApplicationCode,
            AuthKeyValue key)
        {
            RequestingApplicationCode = requestingApplicationCode;
            TargetApplicationCode = targetApplicationCode;
            Key = key;
        }
    }
}
