using KeyChecker.Application.Infrastructure;
using KeyChecker.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KeyChecker.Xunit.Tests
{
    public class GetPermitedApplicationsTests : IClassFixture<CommonHelper>
    {
        private readonly CommonHelper _commonHelper;

        public GetPermitedApplicationsTests(CommonHelper commonHelper)
        {
            _commonHelper = commonHelper;
        }

        #region ApplicationCode tests
        [Fact]
        public async Task GetApp_RequestedAppNotFound_ReturnEmptyList()
        {
            // ARRANGE
            ApplicationCode applicationCode = "1";

            // настраиваем репу приложения, чтобы оно не находило ничего
            var appRepoMock = new Mock<IApplicationRepository>();
            appRepoMock.Setup(x => x.GetApplicationByCodeAsync(applicationCode, default))
                .Returns(() => Task.FromResult(KeyApplication.NoApplication));

            var validator = _commonHelper.InitValidator(appRepoMock.Object);

            // ACT

            var result = await validator.GetPermittedApplicationsAsync(applicationCode);

            // ASSERT

            Assert.True(_commonHelper.CheckIsEmpty(result));
        }

        [Fact]
        public async Task GetApp_2BindedApp1Disabled_Return1ActiveApp()
        {
            // ARRANGE
            ApplicationCode applicationCode = "1";
            KeyApplication application = new KeyApplication(applicationCode, Guid.NewGuid());
            var activeBindedApp = new KeyApplication("2", Guid.NewGuid());
            var disabledBindedApp = new KeyApplication("3", Guid.NewGuid());

            KeyApplication[] bindedApplications = {activeBindedApp, disabledBindedApp};

            // настраиваем репу приложения, находим наше приложение
            // а так же возвращаем список из двух приложений
            var appRepoMock = new Mock<IApplicationRepository>();
            appRepoMock.Setup(x => x.GetApplicationByCodeAsync(applicationCode, default))
                .Returns(() => Task.FromResult(application));

            appRepoMock.Setup(x => x.GetKnownApplicationsAsync(application, default))
                .Returns(() => Task.FromResult<IEnumerable<KeyApplication>>(bindedApplications));

            // настраиваем реп ключей, чтобы он для одного из аппов не нашёл активного ключа
            var keyRepoMock = new Mock<IKeyRepository>();
            keyRepoMock.Setup(x => x.IsHaveActiveAuthKey(activeBindedApp, application, default))
                .Returns(() => new ValueTask<bool>(Task.FromResult(true)));

            keyRepoMock.Setup(x => x.IsHaveActiveAuthKey(disabledBindedApp, application, default))
                .Returns(() => new ValueTask<bool>(Task.FromResult(false)));

            var validator = _commonHelper.InitValidator(appRepoMock.Object, keyRepoMock.Object);

            // ACT

            var result = await validator.GetPermittedApplicationsAsync(applicationCode);

            // ASSERT

            KeyApplication[] expectedResult = { activeBindedApp };

            Assert.True(_commonHelper.CheckIsApplicationListsEqual(result, expectedResult));
        }

        #endregion

        #region ApplicationUid tests
        [Fact]
        public async Task GetAppForUid_RequestedAppNotFound_ReturnEmptyList()
        {
            // ARRANGE
            Guid applicationUid = Guid.NewGuid();

            // настраиваем репу приложения, чтобы оно не находило ничего
            var appRepoMock = new Mock<IApplicationRepository>();
            appRepoMock.Setup(x => x.GetApplicationByUidAsync(applicationUid, default))
                .Returns(() => Task.FromResult(KeyApplication.NoApplication));

            var validator = _commonHelper.InitValidator(appRepoMock.Object);

            // ACT

            var result = await validator.GetPermittedApplicationsAsync(applicationUid);

            // ASSERT

            Assert.True(_commonHelper.CheckIsEmpty(result));
        }

        [Fact]
        public async Task GetAppByUid_2BindedApp1Disabled_Return1ActiveApp()
        {
            // ARRANGE
            Guid applicationUid = Guid.NewGuid();
            KeyApplication application = new KeyApplication(Guid.NewGuid().ToString(), Guid.NewGuid());
            var activeBindedApp = new KeyApplication(Guid.NewGuid().ToString(), Guid.NewGuid());
            var disabledBindedApp = new KeyApplication(Guid.NewGuid().ToString(), Guid.NewGuid());

            KeyApplication[] bindedApplications = { activeBindedApp, disabledBindedApp };

            // настраиваем репу приложения, находим наше приложение
            // а так же возвращаем список из двух приложений
            var appRepoMock = new Mock<IApplicationRepository>();
            appRepoMock.Setup(x => x.GetApplicationByUidAsync(applicationUid, default))
                .Returns(() => Task.FromResult(application));

            appRepoMock.Setup(x => x.GetKnownApplicationsAsync(application, default))
                .Returns(() => Task.FromResult<IEnumerable<KeyApplication>>(bindedApplications));

            // настраиваем реп ключей, чтобы он для одного из аппов не нашёл активного ключа
            var keyRepoMock = new Mock<IKeyRepository>();
            keyRepoMock.Setup(x => x.IsHaveActiveAuthKey(activeBindedApp, application, default))
                .Returns(() => new ValueTask<bool>(Task.FromResult(true)));

            keyRepoMock.Setup(x => x.IsHaveActiveAuthKey(disabledBindedApp, application, default))
                .Returns(() => new ValueTask<bool>(Task.FromResult(false)));

            var validator = _commonHelper.InitValidator(appRepoMock.Object, keyRepoMock.Object);

            // ACT

            var result = await validator.GetPermittedApplicationsAsync(applicationUid);

            // ASSERT

            KeyApplication[] expectedResult = { activeBindedApp };

            Assert.True(_commonHelper.CheckIsApplicationListsEqual(result, expectedResult));
        }

        #endregion
    }
}
