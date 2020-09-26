using KeyChecker.Application;
using KeyChecker.Application.Infrastructure;
using KeyChecker.Application.Infrastructure.Models;
using KeyChecker.Application.Models;
using KeyChecker.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace KeyChecker.Xunit.Tests
{
    /// <summary>
    /// ValidateKeyAsync
    /// </summary>
    public class AuthKeyValidatorTests: IClassFixture<CommonHelper>
    {
        private readonly CommonHelper _commonHelper;

        public AuthKeyValidatorTests(CommonHelper commonHelper)
        {
            _commonHelper = commonHelper;
        }

        [Fact]
        public async Task ValidateKey_NoRequestingApplication_ReturnFalse()
        {
            // ARRANGE

            var applicationCode = "1";
            var targetApplicationCode = "2";
            var authKeyValue = "qwerty";

            // ��� ���������� �����������, ����� �� �� ������� ������������� ����������
            var repoMock = new Mock<IApplicationRepository>();
            repoMock.Setup(x => x.GetApplicationByCodeAsync(applicationCode, default))
                .Returns(() => Task.FromResult(KeyApplication.NoApplication));

            repoMock.Setup(x => x.GetApplicationByCodeAsync(targetApplicationCode, default))
                .Returns(() => Task.FromResult(
                    new KeyApplication(targetApplicationCode, Guid.NewGuid())));

            // ��� ������ � ����� ������� �� ������ ����������, ��� ��� �� �������� ������
            var keyRepoMock = new Mock<IKeyRepository>();
            keyRepoMock.Setup(x => x.GetApplicationForKeyAsync(It.IsAny<ApplicationWithKey>(), default))
                .Throws(new Exception("�� �� ������ �������� �� �������� �����"));

            var validator = _commonHelper.InitValidator(repoMock.Object, keyRepoMock.Object);

            // ACT

            var requestModel =
                new ApplicationCodeAuthKeyValidateRequest(applicationCode, targetApplicationCode, authKeyValue);
            var result = await validator.ValidateKeyAsync(requestModel);

            // ASSERT

            Assert.True(result is false);
        }

        [Fact]
        public async Task ValidateKey_NoTargetApplication_ReturnFalse()
        {
            var applicationCode = "1";
            var targetApplicationCode = "2";
            var authKeyValue = "qwerty";

            // ��� ���������� �����������, ����� �� �� ������� ������� ����������
            var repoMock = new Mock<IApplicationRepository>();
            repoMock.Setup(x => x.GetApplicationByCodeAsync(applicationCode, default))
                .Returns(() => Task.FromResult(
                    new KeyApplication(applicationCode, Guid.NewGuid())));

            repoMock.Setup(x => x.GetApplicationByCodeAsync(targetApplicationCode, default))
                .Returns(() => Task.FromResult(KeyApplication.NoApplication));

            // ��� ������ � ����� ������� �� ������ ����������, ��� ��� �� �������� ������
            var keyRepoMock = new Mock<IKeyRepository>();
            keyRepoMock.Setup(x => x.GetApplicationForKeyAsync(It.IsAny<ApplicationWithKey>(), default))
                .Throws(new Exception("�� �� ������ �������� �� �������� �����"));

            var validator = _commonHelper.InitValidator(repoMock.Object, keyRepoMock.Object);

            var requestModel =
                new ApplicationCodeAuthKeyValidateRequest(applicationCode, targetApplicationCode, authKeyValue);
            var result = await validator.ValidateKeyAsync(requestModel);

            Assert.True(result is false);
        }

        [Fact]
        public async Task ValidateKey_NoKey_ReturnNoAuthKey()
        {
            // ARRANGE

            var applicationCode = "1";
            var targetApplicationCode = "2";
            var authKeyValue = "qwerty";

            // ��� ���������� �����������, ����� �� ������� ��� ����������
            var repoMock = new Mock<IApplicationRepository>();
            repoMock.Setup(x => x.GetApplicationByCodeAsync(applicationCode, default))
                .Returns(() =>
                Task.FromResult(new KeyApplication(applicationCode, Guid.NewGuid())));

            repoMock.Setup(x => x.GetApplicationByCodeAsync(targetApplicationCode, default))
                .Returns(() =>
                Task.FromResult(new KeyApplication(targetApplicationCode, Guid.NewGuid())));

            // ��� ������ ������ �� �������� ������ �����
            var keyRepoMock = new Mock<IKeyRepository>();
            keyRepoMock.Setup(x => x.GetApplicationForKeyAsync(It.IsAny<ApplicationWithKey>(), default))
                .Returns(() => Task.FromResult<AuthKey>(AuthKey.NoKey));

            var validator = _commonHelper.InitValidator(repoMock.Object, keyRepoMock.Object);

            // ACT

            var requestModel =
                new ApplicationCodeAuthKeyValidateRequest(applicationCode, targetApplicationCode, authKeyValue);
            var result = await validator.ValidateKeyAsync(requestModel);

            // ASSERT

            Assert.True(result is false);
        }

        /// <summary>
        /// ������� ��� ��� �������� ����� ��������� ������� �� ������������ �����
        /// </summary>
        [Theory(DisplayName = "ValidateKey_KeyStatus")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ValidateKey_KeyFound(bool keyIsEnabled)
        {
            // ARRANGE

            var applicationCode = "1";
            var application = new KeyApplication(applicationCode, Guid.NewGuid());

            var targetApplicationCode = "2";
            var targetApplication = new KeyApplication(targetApplicationCode, Guid.NewGuid());

            var authKeyValue = "qwerty";

            // ��� ���������� �����������, ����� �� ������� ��� ����������
            var repoMock = new Mock<IApplicationRepository>();
            repoMock.Setup(x => x.GetApplicationByCodeAsync(applicationCode, default))
                .Returns(() => Task.FromResult(application));

            repoMock.Setup(x => x.GetApplicationByCodeAsync(targetApplicationCode, default))
                .Returns(() => Task.FromResult(targetApplication));

            // ��� ������ ������ �� �������� ������ �����
            var keyRepoMock = new Mock<IKeyRepository>();
            keyRepoMock.Setup(x => x.GetApplicationForKeyAsync(It.IsAny<ApplicationWithKey>(), default))
                .Returns(() => Task.FromResult<AuthKey>(
                    new FoundAuthKey(authKeyValue, keyIsEnabled, application, targetApplication)));

            var validator = _commonHelper.InitValidator(repoMock.Object, keyRepoMock.Object);

            // ACT

            var requestModel =
                new ApplicationCodeAuthKeyValidateRequest(applicationCode, targetApplicationCode, authKeyValue);
            var result = await validator.ValidateKeyAsync(requestModel);

            // ASSERT

            Assert.Equal(keyIsEnabled, result);
        }
    }
}
