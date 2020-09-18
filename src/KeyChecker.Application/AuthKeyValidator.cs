using KeyChecker.Application.Infrastructure;
using KeyChecker.Application.Infrastructure.Models;
using KeyChecker.Application.Models;
using KeyChecker.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KeyChecker.Application
{
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

        public async ValueTask<bool> ValidateKey(
            ApplicationCodeAuthKeyValidateRequest request, 
            CancellationToken token = default)
        {
            if(request is null)
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
                    $"Не смогли найти приложение пытающееся получить доступ к запрашиваемому, " +
                    $"код приложения {request.TargetApplicationCode}");

                return false;
            }

            var applicationKeyValidate = new ApplicationWithKey(application, targetApplication, request.Key);
            var authResult = await _keyRepository.GetApplicationForKeyAsync(applicationKeyValidate, token);

            return authResult switch
            {
                NoAuthKey _ => false,
                FoundAuthKey key => key.Enabled,
                _ => throw new Exception("Не знаю как обработать ключ")
            };
        }
    }
}
