using BasicCoreApi.Errors;
using Common.DTO;
using FluentValidation;

namespace BasicCoreApi.Validators
{
    public class CallbackResponseDtoValidator : AbstractValidator<CallbackResponseDto>
    {
        public CallbackResponseDtoValidator()
        {
            this.RuleFor(model => model.Success)
                .Must(x => x.Equals(true))
                .WithMessage(ApiErrorMessage.CallbackStatusIsFalse);

            this.RuleFor(model => model.Validator)
                .NotEmpty()
                .WithMessage(ApiErrorMessage.CallbackValidatorEmpty);
        }
    }
}
