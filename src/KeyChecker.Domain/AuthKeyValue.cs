using System;

namespace KeyChecker.Domain
{
    public struct AuthKeyValue : IEquatable<AuthKeyValue>
    {
        private readonly string _value;

        public AuthKeyValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Значение ключа не должно быть пустым", nameof(value));
            }

            _value = value;
        }

        public bool Equals(AuthKeyValue other) =>
            _value == other._value;

        public static implicit operator string(AuthKeyValue key) => key._value;
        public static implicit operator AuthKeyValue(string value) => new AuthKeyValue(value);
    }
}
