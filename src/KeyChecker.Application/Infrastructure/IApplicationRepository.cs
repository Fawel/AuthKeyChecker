using KeyChecker.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeyChecker.Application.Infrastructure
{
    /// <summary>
    /// Репозиторий для получения данных приложений
    /// </summary>
    public interface IApplicationRepository
    {
        /// <summary>
        /// Пытаемся получить приложение по коду.
        /// Одному коду соот-ет только одно приложение.
        /// </summary>
        /// <param name="applicationCode">Код по которому ищем приложение</param>
        /// <returns>Приложение, которому соот-ет данный код.
        /// Если такого нет, то возвращается объект типа <see cref="NoKeyApplication"/></returns>
        Task<KeyApplication> GetApplicationByCodeAsync(
            ApplicationCode applicationCode,
            CancellationToken token = default);

        /// <summary>
        /// Пытаемся получить приложение по уникальному идентификатору.
        /// Одному идентификатору соот-ет только одно приложение.
        /// </summary>
        /// <param name="applicationUid">Идентификатор по которому ищем приложение</param>
        /// <returns>Приложение, которому соот-ет данный идентификатор.
        /// Если такого нет, то возвращается объект типа <see cref="NoKeyApplication"/></returns>
        Task<KeyApplication> GetApplicationByUidAsync(Guid applicationUid, CancellationToken token = default);
    }
}
