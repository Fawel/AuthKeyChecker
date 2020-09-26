using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KeyChecker.Application.Infrastructure;
using KeyChecker.Domain;

namespace KeyChecker.Infrastructure.Stubs
{
    /// <summary>
    /// Имплементация-заглушка репозитария приложений
    /// </summary>
    public class ApplicationRepositoryStub : IApplicationRepository
    {
        public Task<IEnumerable<KeyApplication>> GetAllKnownApplicationsAsync(CancellationToken token = default)
        {
            return Task.FromResult(Enumerable.Empty<KeyApplication>());
        }

        public Task<KeyApplication> GetApplicationByCodeAsync(ApplicationCode applicationCode, CancellationToken token = default)
        {
            return Task.FromResult(KeyApplication.NoApplication);
        }

        public Task<KeyApplication> GetApplicationByUidAsync(Guid applicationUid, CancellationToken token = default)
        {
            return Task.FromResult(KeyApplication.NoApplication);
        }

        public Task<IEnumerable<KeyApplication>> GetKnownApplicationsAsync(KeyApplication application, CancellationToken token = default)
        {
            return Task.FromResult(Enumerable.Empty<KeyApplication>());
        }
    }
}
