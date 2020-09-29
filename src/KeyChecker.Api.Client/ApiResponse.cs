namespace KeyChecker.Api.Client
{
    /// <summary>
    /// Обёртка ответа апи
    /// </summary>
    public class ApiResponse
    {
        public readonly bool IsSuccess;
        public readonly string Message;

        private static ApiResponse _cachedSuccessResult = new ApiResponse(true, null);
        private static ApiResponse _cachedNullRequestResponse =
            new ApiResponse(false, "Модель запроса не должна быть null");

        protected ApiResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        /// <summary>
        /// Возвращает модель ответа при провале
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResponse CreateFailed(string message) =>
            new ApiResponse(false, message);

        /// <summary>
        /// Возвращает модель успешного ответа
        /// </summary>
        /// <returns></returns>
        public static ApiResponse CreateSuccess() =>
            _cachedSuccessResult;

        /// <summary>
        /// Возвращает модель ответа, когда модель запроса равна Null
        /// </summary>
        /// <returns></returns>
        public static ApiResponse CreateNullRequestResponse() =>
            _cachedNullRequestResponse;
    }

    /// <summary>
    /// Обёртка ответа апи с полезной нагрузкой от апи
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        private readonly T Data;

        private static ApiResponse<T> _cachedNullRequestResponse =
            new ApiResponse<T>(default, false, "Модель запроса не должна быть null");

        protected ApiResponse(T data, bool isSuccess, string message) :
            base(isSuccess, message)
        {
            Data = data;
        }

        public static new ApiResponse<T> CreateFailed(string message) =>
            new ApiResponse<T>(default, false, message);

        public static ApiResponse<T> CreateSuccess(T data) =>
            new ApiResponse<T>(data, true, null);

        /// <summary>
        /// Возвращает модель ответа, когда модель запроса равна Null
        /// </summary>
        /// <returns></returns>
        public static new ApiResponse<T> CreateNullRequestResponse() =>
            _cachedNullRequestResponse;

        public static implicit operator T (ApiResponse<T> response)
        {
            if(response is null || !response.IsSuccess)
            {
                throw new System.Exception($"Не могу сконвертировать ответ в данные");
            }

            return response.Data;
        }
    }

}
