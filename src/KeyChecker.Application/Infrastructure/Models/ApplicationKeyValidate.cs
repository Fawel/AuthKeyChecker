using KeyChecker.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeyChecker.Application.Infrastructure.Models
{
    public class ApplicationWithKey
    {
        /// <summary>
        /// Приложение запрашивающее проверку
        /// </summary>
        public readonly KeyApplication RequestingApplication;

        /// <summary>
        /// Проверяемый аутентификационный ключ
        /// </summary>
        public readonly AuthKeyValue Key;

        /// <summary>
        /// Приложение пытающееся получить доступ к запрашивающему проверку
        /// </summary>
        public readonly KeyApplication TargetApplication;

        public ApplicationWithKey(
            KeyApplication requestingApplication,
            KeyApplication targetApplication,
            AuthKeyValue key)
        {
            RequestingApplication = requestingApplication ?? throw new ArgumentNullException(nameof(requestingApplication));
            Key = key;
            TargetApplication = targetApplication ?? throw new ArgumentNullException(nameof(targetApplication));
        }
    }
}
