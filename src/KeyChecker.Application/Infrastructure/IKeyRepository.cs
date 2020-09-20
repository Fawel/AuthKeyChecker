using KeyChecker.Application.Infrastructure.Models;
using KeyChecker.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace KeyChecker.Application.Infrastructure
{
    /// <summary>
    /// Репозиторий для ключей приложений
    /// </summary>
    public interface IKeyRepository
    {
        /// <summary>
        /// Пытаемся получить приложение по указанному ключу.
        /// Каждый ключ соот-ет только одному приложению одновременно
        /// </summary>
        /// <param name="key">Аутентификационный ключ и приложение, которое запрашивает проверку</param>
        /// <returns>Информация об аутентификационном ключе</returns>
        Task<AuthKey> GetApplicationForKeyAsync(ApplicationWithKey key, CancellationToken token = default);
        
        /// <summary>
        /// Узнает есть ли хотя бы один включенный аутентификационный ключ между двумя приложениями
        /// </summary>
        /// <param name="application">Проверяемое приложение</param>
        /// <param name="targetApplication">Приложение к которому нужно иметь доступ</param>
        ValueTask<bool> IsHaveActiveAuthKey(
            KeyApplication application,
            KeyApplication targetApplication,
            CancellationToken token = default);
    }
}
