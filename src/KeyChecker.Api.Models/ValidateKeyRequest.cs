using System;

namespace KeyChecker.Api.Models
{
    public class ValidateKeyRequest
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
            if (string.IsNullOrWhiteSpace(applicationCode))
            {
                throw new ArgumentException("Код приложения не должен быть пуст", nameof(applicationCode));
            }

            if (string.IsNullOrWhiteSpace(targetApplicationCode))
            {
                throw new ArgumentException("Код приложения не должен быть пуст", nameof(targetApplicationCode));
            }

            if (string.IsNullOrWhiteSpace(authKeyValue))
            {
                throw new ArgumentException("Код ключа не должен быть пуст", nameof(authKeyValue));
            }

            ApplicationCode = applicationCode;
            TargetApplicationCode = targetApplicationCode;
            AuthKeyValue = authKeyValue;
        }
    }
}
