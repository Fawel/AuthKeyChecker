using System;
using System.Collections.Generic;
using System.Text;

namespace KeyChecker.Domain
{
    /// <summary>
    /// Абстрактный класс аутентификационного ключа между приложениями
    /// </summary>
    public abstract class AuthKey
    {
        public static NoAuthKey NoKey = new NoAuthKey();
    }

    /// <summary>
    /// Существующий аутентификационный ключ между двумя приложениями
    /// </summary>
    public class ExistingAuthKey : AuthKey
    {
        /// <summary>
        /// Значение ключа
        /// </summary>
        public readonly AuthKeyValue KeyValue;

        /// <summary>
        /// Включён ли ключ
        /// </summary>
        public readonly bool Enabled;

        /// <summary>
        /// Приложение запрашивающее доступ к другому
        /// </summary>
        public readonly KeyApplication Application;

        /// <summary>
        /// Приложение к которому запрашивают доступ
        /// </summary>
        public readonly KeyApplication TargetApplication;

        public ExistingAuthKey(
            AuthKeyValue keyValue,
            bool enabled,
            KeyApplication application,
            KeyApplication targetApplication)
        {
            KeyValue = keyValue;
            Enabled = enabled;
            Application = application ?? throw new ArgumentNullException(nameof(application));
            TargetApplication = targetApplication ?? throw new ArgumentNullException(nameof(targetApplication));
        }
    }

    /// <summary>
    /// Ненайденный аутентификационный ключ
    /// </summary>
    public class NoAuthKey : AuthKey
    {
    }
}
