using System;
using System.Collections.Generic;
using System.Text;

namespace KeyChecker.Api.Models
{
    public abstract class BasicRequest
    {
        public abstract bool SelfValidate(out string message);
    }
}
