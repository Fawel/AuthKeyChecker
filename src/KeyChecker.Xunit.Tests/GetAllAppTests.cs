using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using System.Threading.Tasks;
using KeyChecker.Domain;
using KeyChecker.Application.Infrastructure;

namespace KeyChecker.Xunit.Tests
{
    /// <summary>
    /// Тесты метода GetAllKnownApplicationsAsync
    /// </summary>
    public class GetAllAppTests : IClassFixture<CommonHelper>
    {
        private readonly CommonHelper _commonHelper;

        public GetAllAppTests(CommonHelper commonHelper)
        {
            _commonHelper = commonHelper;
        }

        /// <summary>
        /// Ситуация: репозитарий известны два приложения
        /// Ожидаемый результат: метод вернул список из тех же двух известных приложений
        /// </summary>
        [Fact]
        public async Task GetAllApps_ReturnResultFromRepo()
        {
            // ARRANGE

            var firstApp = new KeyApplication(Guid.NewGuid().ToString(), Guid.NewGuid());
            var secondApp = new KeyApplication(Guid.NewGuid().ToString(), Guid.NewGuid());

            KeyApplication[] expectedResult = { firstApp, secondApp };

            // настраиваем реп приложений, чтобы он возвращал список из двух приложений
            var appRepoMock = new Mock<IApplicationRepository>();
            appRepoMock.Setup(x => x.GetAllKnownApplicationsAsync(default))
                .Returns(() => Task.FromResult<IEnumerable<KeyApplication>>(expectedResult));

            var validator = _commonHelper.InitValidator(appRepoMock.Object);

            // ACT

            var result = await validator.GetAllKnownApplicationsAsync();

            // ASSERT

            Assert.True(_commonHelper.CheckIsApplicationListsEqual(expectedResult, result));
        }
    }
}
