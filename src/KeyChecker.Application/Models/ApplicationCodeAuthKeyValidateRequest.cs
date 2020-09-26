using KeyChecker.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeyChecker.Application.Models
{
    /// <summary>
    /// Модель запроса наличия доступа по ключу между двумя приложениями
    /// </summary>
    public class ApplicationCodeAuthKeyValidateRequest
    {
        /// <summary>
        /// Приложение, пытающееся получить доступ к другому
        /// </summary>
        public readonly ApplicationCode RequestingApplicationCode;

        /// <summary>
        /// Приложение, к которому пытаются получить доступ
        /// </summary>
        public readonly ApplicationCode TargetApplicationCode;

        /// <summary>
        /// Значение ключа между двумя приложениями
        /// </summary>
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
