using System.Threading;
using System.Threading.Tasks;
using KeyChecker.Application.Infrastructure;
using KeyChecker.Application.Infrastructure.Models;
using KeyChecker.Domain;

namespace KeyChecker.Infrastructure.Stubs
{
    /// <summary>
    /// Имплементация-заглушка репозитория ключей
    /// </summary>
    public class KeyRepositoryStub : IKeyRepository
    {
        public Task<AuthKey> GetApplicationForKeyAsync(ApplicationWithKey key, CancellationToken token = default)
        {
            return Task.FromResult<AuthKey>(AuthKey.NoKey);
        }

        public ValueTask<bool> IsHaveActiveAuthKey(KeyApplication application, KeyApplication targetApplication, CancellationToken token = default)
        {
            return new ValueTask<bool>(Task.FromResult(false));
        }
    }
}
