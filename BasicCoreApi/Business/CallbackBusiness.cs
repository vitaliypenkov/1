using BasicCoreApi.Helpers;
using BasicCoreApi.Validators;
using Common.DTO;
using FluentValidation.Results;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BasicCoreApi.Business
{
    public class CallbackBusiness : ICallbackBusiness
    {
        private readonly HttpClient HttpClient;

        public CallbackBusiness(IConfiguration configuration)
        { 
            this.HttpClient = new()
            {
                Timeout = TimeSpan.FromSeconds(configuration.GetValue<int>(ApiConstants.ApiConstants.CALLBACK_TIMEOUT_SECONDS)),
            };
        }

        public async Task<ValidationResult> MakeCallbackAsync(CallbackRequestDto callbackRequestDto, string callbackUrl)
        {
            var regeneratedUserId = CryptoHelper.Encrypt(callbackRequestDto.UserId);
                
            using HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, callbackUrl);
            httpRequestMessage.Headers.Add(HeaderNames.Authorization, string.Format(ApiConstants.ApiConstants.AUTHORIZATION_HEADER_FORMAT, regeneratedUserId));

            StringContent stringContent = new(JsonSerializer.Serialize(callbackRequestDto), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(callbackUrl, stringContent);

            JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
            var callbackResponseDto = await JsonSerializer.DeserializeAsync<CallbackResponseDto>(httpResponseMessage.Content.ReadAsStream(), jsonSerializerOptions);

            // TODO: Handle callbackResponseDto being null.
            return new CallbackResponseDtoValidator().Validate(callbackResponseDto);
        }
    }
}
