using KeyChecker.Application.Infrastructure;
using KeyChecker.Application.Infrastructure.Models;
using KeyChecker.Application.Models;
using KeyChecker.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KeyChecker.Application
{
    /// <summary>
    /// Сервис для проверки доступа приложений друг к другу по аутентификационному ключу
    /// а также получения списка известных приложений с наличием такого доступа
    /// </summary>
    public class AuthKeyValidator
    {
        private readonly IKeyRepository _keyRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILogger<AuthKeyValidator> _logger;

        // статичные тексты ошибок
        private readonly string requestBodyIsNull = "В запрос проверки ключа не передано тело запроса";

        public AuthKeyValidator(
            IKeyRepository keyRepository,
            IApplicationRepository applicationRepository,
            ILogger<AuthKeyValidator> logger)
        {
            _keyRepository = keyRepository ?? throw new ArgumentNullException(nameof(keyRepository));

            _applicationRepository = applicationRepository ??
                throw new ArgumentNullException(nameof(applicationRepository));

            _logger = logger;
        }

        #region Public методы, они же - методы приложения
        /// <summary>
        /// Проверяет что предоставленный ключ от приложения делающего запрос
        /// существует целевого приложения
        /// </summary>
        public async ValueTask<bool> ValidateKeyAsync(
            ApplicationCodeAuthKeyValidateRequest request,
            CancellationToken token = default)
        {
            if (request is null)
            {
                _logger?.LogError(requestBodyIsNull);
                throw new ArgumentNullException(nameof(request), requestBodyIsNull);
            }

            var application =
                await _applicationRepository.GetApplicationByCodeAsync(request.RequestingApplicationCode, token);

            if (application is NoKeyApplication)
            {
                _logger?.LogWarning(
                    $"Не смогли найти запрашивающее приложение по ключу {request.RequestingApplicationCode}");

                return false;
            }

            var targetApplication =
                await _applicationRepository.GetApplicationByCodeAsync(request.TargetApplicationCode, token);

            if (targetApplication is NoKeyApplication)
            {
                _logger?.LogWarning(
                    $"Не смогли найти целевое приложение, " +
                    $"код приложения {request.TargetApplicationCode}");

                return false;
            }

            var applicationKeyValidate = new ApplicationWithKey(application, targetApplication, request.Key);
            var authResult = await _keyRepository.GetApplicationForKeyAsync(applicationKeyValidate, token);

            return authResult switch
            {
                NoAuthKey _ => false,
                ExistingAuthKey key => key.Enabled,
                _ => throw new Exception("Не знаю как обработать ключ")
            };
        }

        /// <summary>
        /// Отдаёт список известных приложений
        /// </summary>
        /// <returns>Список кодов и uid зарегестрированых приложений</returns>
        public async Task<IEnumerable<KeyApplication>> GetAllKnownApplicationsAsync(CancellationToken token = default)
            => await _applicationRepository.GetAllKnownApplicationsAsync(token);

        /// <summary>
        /// Получает список приложений имеющих аутентификационные ключи к приложению указанному в аргументе
        /// </summary>
        /// <param name="applicationCode">Код приложения для которого получаем список приложений 
        /// которые могут сделать к нему запрос</param>
        /// <returns>Список кодов и uid зарегестрированых приложений</returns>
        public async Task<IEnumerable<KeyApplication>> GetPermittedApplicationsAsync(
            ApplicationCode applicationCode,
            CancellationToken token = default)
        {
            var application = await _applicationRepository.GetApplicationByCodeAsync(applicationCode, token);

            if (application is NoKeyApplication)
                return Enumerable.Empty<KeyApplication>();

            return await GetRegisteredApplicationsAsync(application, token);
        }

        /// <summary>
        /// Получает список приложений имеющих аутентификационные активные ключи к приложению указанному в аргументе
        /// </summary>
        /// <param name="applicationUid">Uid приложения для которого получаем список приложений</param>
        /// <returns>Список кодов и uid зарегестрированых приложений</returns>
        public async Task<IEnumerable<KeyApplication>> GetPermittedApplicationsAsync(
            Guid applicationUid,
            CancellationToken token = default)
        {
            var application = await _applicationRepository.GetApplicationByUidAsync(applicationUid, token);

            if (application is NoKeyApplication)
                return Enumerable.Empty<KeyApplication>();

            return await GetRegisteredApplicationsAsync(application, token);
        }
        #endregion

        #region Внутренние методы приложения
        /// <summary>
        /// Получаем связанные приложения с приложением из аргумента
        /// Связанное приложение должно иметь при этом активный аутентификационный ключ
        /// </summary>
        /// <param name="application">Приложение к которому ищем связанные приложения</param>
        private async Task<KeyApplication[]> GetRegisteredApplicationsAsync(
            KeyApplication application,
            CancellationToken token = default)
        {
            var bindedApplicationList = (await _applicationRepository.GetKnownApplicationsAsync(application, token))
                .ToArray();

            // фильтруем аппы, нам нужны только те, кто имеет включенный ключ

            var result = new List<KeyApplication>(bindedApplicationList.Length);
            foreach (var bindedApplication in bindedApplicationList)
            {
                if (await _keyRepository.IsHaveActiveAuthKey(bindedApplication, application, token))
                {
                    result.Add(bindedApplication);
                }
            }

            return result.ToArray();
        } 
        #endregion
    }
}
