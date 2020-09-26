using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KeyChecker.Application.Infrastructure;
using KeyChecker.Domain;

namespace KeyChecker.Infrastructure.TestImplementation
{
    /// <summary>
    /// Тестовая имплементация репозитория приложений в памяти
    /// </summary>
    public class InMemoryApplicationRepository : IApplicationRepository
    {
        private readonly Dictionary<ApplicationCode, KeyApplication> _codeDictionary =
            new Dictionary<ApplicationCode, KeyApplication>();

        private readonly Dictionary<Guid, KeyApplication> _uidDictionary =
            new Dictionary<Guid, KeyApplication>();

        private readonly Dictionary<KeyApplication, KeyApplication[]> _bindedAppDictionary =
            new Dictionary<KeyApplication, KeyApplication[]>();

        public InMemoryApplicationRepository()
        {
            int index = 1;
            // заполняем приложениями
            foreach (var app in TestApplicationCollection.KeyApplications)
            {
                _codeDictionary.Add(app.Code, app);
                _uidDictionary.Add(app.Uid, app);

                // "рисуем" связи между приложениями
                var bindedApplications = 
                    new KeyApplication[TestApplicationCollection.KeyApplications.Length - index];

                for (int i = index; i < bindedApplications.Length; i++)
                {
                    bindedApplications[i] = TestApplicationCollection.KeyApplications[i];
                }

                _bindedAppDictionary.Add(app, bindedApplications);
                index++;
            }
        }

        public Task<IEnumerable<KeyApplication>> GetAllKnownApplicationsAsync(CancellationToken token = default)
        {
            var result = new List<KeyApplication>(_codeDictionary.Count);
            foreach (var app in _codeDictionary)
            {
                result.Add(app.Value);
            }

            return Task.FromResult<IEnumerable<KeyApplication>>(result);
        }

        public Task<KeyApplication> GetApplicationByCodeAsync(
            ApplicationCode applicationCode,
            CancellationToken token = default)
        {
            if(!_codeDictionary.TryGetValue(applicationCode, out var app))
            {
                return Task.FromResult(KeyApplication.NoApplication);
            }

            return Task.FromResult(app);
        }

        public Task<KeyApplication> GetApplicationByUidAsync(Guid applicationUid, CancellationToken token = default)
        {
            if (!_uidDictionary.TryGetValue(applicationUid, out var app))
            {
                return Task.FromResult(KeyApplication.NoApplication);
            }

            return Task.FromResult(app);
        }

        public Task<IEnumerable<KeyApplication>> GetKnownApplicationsAsync(
            KeyApplication application,
            CancellationToken token = default)
        {
            if(!_bindedAppDictionary.TryGetValue(application, out var bindedApps))
            {
                var emptyResult = new KeyApplication[] { KeyApplication.NoApplication };
                return Task.FromResult<IEnumerable<KeyApplication>>(emptyResult);
            }

            return Task.FromResult<IEnumerable<KeyApplication>>(bindedApps);
        }
    }
}
