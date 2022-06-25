using Common.DTO;
using FluentValidation.Results;

namespace BasicCoreApi.Business
{
    public interface ICallbackBusiness
    {
        Task<ValidationResult> MakeCallbackAsync(CallbackRequestDto callbackRequestDto, string callbackUrl);       
    }
}
