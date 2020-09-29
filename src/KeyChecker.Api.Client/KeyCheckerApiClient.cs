using Jil;
using KeyChecker.Api.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeyChecker.Api.Client
{
    public class KeyCheckerApiClient
    {
        private readonly HttpClient _httpClient;

        public KeyCheckerApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ApiResponse<bool>> ValidateApplicationKey(
            ValidateKeyRequest request, 
            CancellationToken token = default)
        {
            if (request is null)
            {
                return ApiResponse<bool>.CreateNullRequestResponse();
            }

            // проверяем модель запроса
            if (!request.SelfValidate(out var validateMessage))
            {
                return ApiResponse<bool>.CreateFailed(validateMessage);
            }

            var json = JSON.Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            using var apiResponse = await _httpClient.PostAsync("auth/validate", payload);
            
            if (!apiResponse.IsSuccessStatusCode)
            {
                var message = await apiResponse.Content.ReadAsStringAsync();
                message = !string.IsNullOrWhiteSpace(message) ? message : apiResponse.StatusCode.ToString();
                return ApiResponse<bool>.CreateFailed(message);
            }

            using TextReader streamReader = new StreamReader(await apiResponse.Content.ReadAsStreamAsync());
            var resultData = Jil.JSON.Deserialize<bool>(streamReader);
            return ApiResponse<bool>.CreateSuccess(resultData);
        }
    }
}
