using System;
using System.Text;

namespace KeyChecker.Api.Models
{
    public class ValidateKeyRequest : BasicRequest
    {
        public string ApplicationCode { get; set; }
        public string TargetApplicationCode { get; set; }
        public string AuthKeyValue { get; set; }

        public ValidateKeyRequest()
        {

        }

        public ValidateKeyRequest(
            string applicationCode,
            string targetApplicationCode,
            string authKeyValue)
        {
            ApplicationCode = applicationCode;
            TargetApplicationCode = targetApplicationCode;
            AuthKeyValue = authKeyValue;
        }

        public override bool SelfValidate(out string message)
        {
            StringBuilder messageBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(ApplicationCode))
            {
                messageBuilder.Append("Код приложения не должен быть пуст");
            }

            if (string.IsNullOrWhiteSpace(TargetApplicationCode))
            {
                messageBuilder.Append("Код целевого приложения не должен быть пуст");
            }

            if (string.IsNullOrWhiteSpace(AuthKeyValue))
            {
                messageBuilder.Append("Код ключа не должен быть пуст");
            }

            message = messageBuilder.ToString();
            return string.IsNullOrWhiteSpace(message);
        }
    }
}
