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

        /// <summary>
        /// Возвращает список известных всех приложений
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Коды и uid известных приложений</returns>
        Task<IEnumerable<KeyApplication>> GetAllKnownApplicationsAsync(CancellationToken token = default);

        /// <summary>
        /// Получает список приложений имеющих аутентификационные ключи к приложению указанному в аргументе
        /// </summary>
        /// <param name="application">Приложение для которого получаем список приложений 
        /// которые могут сделать к нему запрос</param>
        /// <returns>Список кодов и uid зарегестрированых приложений</returns>
        Task<IEnumerable<KeyApplication>> GetKnownApplicationsAsync(
            KeyApplication application,
            CancellationToken token = default);
    }
}
