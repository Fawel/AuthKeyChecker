using System;
using System.Threading.Tasks;
using AutoMapper;
using KeyChecker.Api.Models;
using KeyChecker.Application;
using KeyChecker.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KeyChecker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthKeyValidator _authKeyValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            AuthKeyValidator authKeyValidator,
            IMapper mapper,
            ILogger<AuthController> logger)
        {
            _authKeyValidator = authKeyValidator ?? throw new ArgumentNullException(nameof(authKeyValidator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateKey([FromBody]ValidateKeyRequest validateKeyRequest)
        {
            // валидация модели запроса
            if (validateKeyRequest is null)
            {
                _logger.LogWarning("В запрос на валидацию пришла пустая модель");
                return BadRequest("В запрос пришла пустая модель");
            }

            else if (!validateKeyRequest.SelfValidate(out var validateMessage))
            {
                _logger.LogWarning(validateMessage);
                return BadRequest(validateMessage);
            }

            // собственно выполнение запроса

            var validateApplicationRequest = _mapper.Map<ApplicationCodeAuthKeyValidateRequest>(validateKeyRequest);

            var result = await _authKeyValidator.ValidateKeyAsync(validateApplicationRequest);
            return Ok(result);
        }
    }
}
