using System;
using System.Collections.Generic;
using KeyChecker.Application.Infrastructure;
using System.Text;
using System.Threading.Tasks;
using KeyChecker.Domain;
using KeyChecker.Application.Infrastructure.Models;
using System.Threading;

namespace KeyChecker.Infrastructure.TestImplementation
{
    /// <summary>
    /// Тестовая имплементация репозитория ключей в памяти
    /// </summary>
    public class InMemoryKeyRepository : IKeyRepository
    {
        private readonly Dictionary<ApplicationWithKey, AuthKey> _keyDictionary =
            new Dictionary<ApplicationWithKey, AuthKey>();

        public InMemoryKeyRepository()
        {
            int index = 1;
            // заполняем приложениями
            foreach (var app in TestApplicationCollection.KeyApplications)
            {
                for (int i = index; i < TestApplicationCollection.KeyApplications.Length - index; i++)
                {
                    AuthKeyValue authKeyValue = $"{index}";
                    var appKey = new ApplicationWithKey(
                        TestApplicationCollection.KeyApplications[i],
                        app,
                        authKeyValue);

                    AuthKey key = new ExistingAuthKey(
                        authKeyValue,
                        true,
                        TestApplicationCollection.KeyApplications[i],
                        app);

                    _keyDictionary.Add(appKey, key);
                }
            }
        }

        public Task<AuthKey> GetApplicationForKeyAsync(ApplicationWithKey key, CancellationToken token = default)
        {
            if (!_keyDictionary.TryGetValue(key, out var authKeyApplication))
            {
                return Task.FromResult<AuthKey>(AuthKey.NoKey);
            }

            return Task.FromResult(authKeyApplication);
        }

        public ValueTask<bool> IsHaveActiveAuthKey(KeyApplication application, KeyApplication targetApplication, CancellationToken token = default)
        {
            foreach(var appKeyRow in _keyDictionary)
            {
                var authKey = (ExistingAuthKey)appKeyRow.Value;
                var app = appKeyRow.Key;

                if(app.TargetApplication != targetApplication &&
                    app.RequestingApplication != application)
                {
                    continue;
                }

                if (authKey.Enabled)
                {
                    return new ValueTask<bool>(Task.FromResult(true));
                }
            }

            return new ValueTask<bool>(false);
        }
    }
}
