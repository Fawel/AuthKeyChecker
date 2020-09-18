using System;
using System.Collections.Generic;
using System.Text;

namespace KeyChecker.Domain
{
    public struct ApplicationCode
    {
        private readonly string _value;

        public ApplicationCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Значение кода приложения не должно быть пустым", nameof(value));
            }

            _value = value;
        }

        public bool Equals(ApplicationCode other) =>
            _value == other._value;

        public static implicit operator string(ApplicationCode key) => key._value;
        public static implicit operator ApplicationCode(string value) => new ApplicationCode(value);
    }
}
