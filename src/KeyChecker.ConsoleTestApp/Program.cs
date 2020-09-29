using System;
using System.Threading.Tasks;
using KeyChecker.Application;
using KeyChecker.Application.Models;
using KeyChecker.Application.Infrastructure;
using KeyChecker.Infrastructure.TestImplementation;
using KeyChecker.Api.Client;
using KeyChecker.Api.Models;

namespace KeyChecker.ConsoleTestApp
{
    class Program
    {
        private readonly AuthKeyValidator _keyValidator;
        private readonly KeyCheckerApiClient _keyCheckerApiClient;

        public Program(AuthKeyValidator authKeyValidator, KeyCheckerApiClient keyCheckerApiClient)
        {
            _keyValidator = authKeyValidator;
            _keyCheckerApiClient = keyCheckerApiClient;
        }

        static async Task Main(string[] args)
        {
            // Poor man DI
            IKeyRepository keyRepository = new InMemoryKeyRepository();
            IApplicationRepository appRepo = new InMemoryApplicationRepository();

            var httpClient = new System.Net.Http.HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:51313");

            var apiClient = new KeyCheckerApiClient(httpClient);

            var authKeyValidator = new AuthKeyValidator(keyRepository, appRepo, null);

            var program = new Program(authKeyValidator, apiClient);

            // Собственно выполнение самой проги
            await program.ExecuteTestSequenceAsync();
            Console.ReadLine();
        }

        private async Task ExecuteTestSequenceAsync()
        {
            // тесты самого сервиса
            var result = await _keyValidator.GetAllKnownApplicationsAsync();
            var isValid = await _keyValidator.GetPermittedApplicationsAsync("Tort");

            var request = new ApplicationCodeAuthKeyValidateRequest("Nep", "Tort", "1");
            var validateKey = await _keyValidator.ValidateKeyAsync(request);

            //тесты апи
            var apiRequest = new ValidateKeyRequest("Nep", "Tort", "1");
            var isValidFromApi = await _keyCheckerApiClient.ValidateApplicationKey(apiRequest);
            if(isValidFromApi)
            {
                Console.WriteLine("Апи сработало. Ну и отлично.");
            }
        }
    }
}
