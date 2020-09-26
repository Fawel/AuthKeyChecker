using System;
using System.Collections.Generic;
using System.Text;
using KeyChecker.Application;
using KeyChecker.Application.Infrastructure;
using KeyChecker.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace KeyChecker.Xunit.Tests
{
    public class CommonHelper
    {
        /// <summary>
        /// Создаёт экземпляр AuthKeyValidator.
        /// Если не указано иного, заполняет все зависимости моками по умолчанию
        /// </summary>
        /// <param name="appRepo">Реализация репозитария приложений</param>
        /// <param name="keyRepo">Реализация репозитария аутентификационных ключей</param>
        /// <param name="logger">Реализация логгера</param>
        public AuthKeyValidator InitValidator(
            IApplicationRepository appRepo = default,
            IKeyRepository keyRepo = default,
            ILogger<AuthKeyValidator> logger = default)
        {
            var repoMock = appRepo ?? Mock.Of<IApplicationRepository>();
            var keyMock = keyRepo ?? Mock.Of<IKeyRepository>();
            var loggerMock = logger ?? Mock.Of<ILogger<AuthKeyValidator>>();

            return new AuthKeyValidator(keyMock, repoMock, loggerMock);
        }

        /// <summary>
        /// Проверяет пуст ли список
        /// </summary>
        public bool CheckIsEmpty<T>(IEnumerable<T> enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            enumerator.Reset();
            return !enumerator.MoveNext();
        }

        public bool CheckIsApplicationListsEqual(
            IEnumerable<KeyApplication> firstApplicationList,
            IEnumerable<KeyApplication> secondApplicationList)
        {
            if (firstApplicationList is null ||
                secondApplicationList is null)
            {
                throw new ArgumentNullException("Оба проверяемых списка не должны равняться нулю");
            }

            var firstArray = ConvertIEnumerableToArray(firstApplicationList);
            var secondArray = ConvertIEnumerableToArray(secondApplicationList);

            if(firstArray.Length != secondArray.Length)
            {
                return false;
            }

            // В теории, в списке приложений не должно быть повторяющихся элементов
            var foundResults = new Dictionary<KeyApplication, bool>();

            for (int firstIndex = 0; firstIndex < firstArray.Length; firstIndex++)
            {
                var currentApp = firstArray[firstIndex];
                if (foundResults.ContainsKey(currentApp))
                {
                    continue;
                }

                for (int secondIndex = 0; secondIndex < secondArray.Length; secondIndex++)
                {
                    var appInSecondList = secondArray[secondIndex];
                    if(currentApp == appInSecondList)
                    {
                        foundResults.Add(currentApp, true);
                        break;
                    }
                }

                if (foundResults.ContainsKey(currentApp))
                {
                    continue;
                }

                // сюда мы добираемся, только если не нашли элемента
                return false;
            }

            return true;
        }

        private T[] ConvertIEnumerableToArray<T>(IEnumerable<T> enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            enumerator.Reset();

            var list = new List<T>();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }

            return list.ToArray();
        }
    }
}
