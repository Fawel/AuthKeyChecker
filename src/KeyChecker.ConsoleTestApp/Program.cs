using System;
using System.Threading.Tasks;
using KeyChecker.Application;
using KeyChecker.Infrastructure;
using KeyChecker.Application.Models;
using KeyChecker.Application.Infrastructure;
using KeyChecker.Infrastructure.TestImplementation;

namespace KeyChecker.ConsoleTestApp
{
    class Program
    {
        private readonly AuthKeyValidator _keyValidator;

        public Program(AuthKeyValidator authKeyValidator)
        {
            _keyValidator = authKeyValidator;
        }

        static async Task Main(string[] args)
        {
            // Poor man DI
            IKeyRepository keyRepository = new InMemoryKeyRepository();
            IApplicationRepository appRepo = new InMemoryApplicationRepository();

            AuthKeyValidator authKeyValidator = new AuthKeyValidator(keyRepository, appRepo, null);

            Program program = new Program(authKeyValidator);

            // Собственно выполнение самой проги
            await program.ExecuteTestSequenceAsync();
            Console.ReadLine();
        }

        private async Task ExecuteTestSequenceAsync()
        {
            var result = await _keyValidator.GetAllKnownApplicationsAsync();
            var isValid = await _keyValidator.GetPermittedApplicationsAsync("Tort");

            var request = new ApplicationCodeAuthKeyValidateRequest("Nep", "Tort", "1");
            var validateKey = await _keyValidator.ValidateKeyAsync(request);
        }
    }
}
