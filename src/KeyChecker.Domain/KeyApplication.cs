using System;
using System.Collections.Generic;
using System.Text;

namespace KeyChecker.Domain
{
    /// <summary>
    /// Приложение с ограниченным по ключу доступом
    /// </summary>
    public class KeyApplication
    {
        /// <summary>
        /// Человекочитабельный идентификатор приложения
        /// </summary>
        public readonly ApplicationCode Code;

        /// <summary>
        /// Уникальный uid приложения
        /// </summary>
        public readonly Guid Uid;

        public readonly static KeyApplication NoApplication = new NoKeyApplication();

        public KeyApplication(ApplicationCode code, Guid uid)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Код приложения должно быть передан", nameof(code));
            }

            Code = code;
            Uid = uid;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, Uid);
        }

        public override bool Equals(object obj)
        {
            if(!(obj is KeyApplication))
            {
                return false;
            }

            var convertedObj = (KeyApplication)obj;
            return Uid == convertedObj.Uid &&
                Code == convertedObj.Code;
        }
    }

    /// <summary>
    /// Объект ненайденного приложения с доступом по ключу
    /// </summary>
    public class NoKeyApplication : KeyApplication
    {
        private readonly static ApplicationCode _noAppName = "noApp";
        public NoKeyApplication() : base(_noAppName, default){ }
    }
}
