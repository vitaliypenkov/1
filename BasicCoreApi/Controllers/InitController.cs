using BasicCoreApi.Business;
using BasicCoreApi.Errors;
using BasicCoreApi.Helpers;
using Common;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace BasicCoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InitController : ControllerBase
    {
        private const string Pattern = "AES .{88}";

        /// <summary>
        /// As per the Authorization pattern it starts with "AES " value followed by the actual encrypted data.
        /// Data starts at the specified index.
        /// </summary>
        private const int UserDataStartIndex = 3;

        private readonly ICallbackBusiness callbackBusiness;

        public InitController(ICallbackBusiness callbackBusiness)
        {
            this.callbackBusiness = callbackBusiness;
        }

        [HttpPost(Name = "init")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InitResponseDto>> InitAsync(
            [FromQuery(Name = "callback")][Required] string callbackUrl, 
            [FromHeader(Name = "Authorization")][Required] string userId,
            [FromHeader(Name = "User-Agent")][Required] string validationHash)
        {

            bool countryCookieExists = Request.Cookies.TryGetValue(Constants.COUNTRY_COOKIE, out var countryCode);
            if (!countryCookieExists)
            {
                return BadRequest(new ErrorResponse((int)ApiErrorCode.MissingCountryCookie, ApiErrorMessage.MissingCountryCookie));
            }

            if (!Regex.IsMatch(userId, Pattern))
            {
                return BadRequest(new ErrorResponse((int)ApiErrorCode.AutorizationHeadearMalformed, ApiErrorMessage.CorrectAutorizationHeaderFormat));
            }

            string decryptedUserId;
            try
            {
                decryptedUserId = CryptoHelper.Decrypt(userId[UserDataStartIndex..]);
            }
            catch (FormatException ex)
            {
                return BadRequest(new ErrorResponse((int)ApiErrorCode.AutorizationHeadearMalformed, ex.Message));
            }

            CallbackRequestDto callbackRequestDto = new()
            { 
                Country = countryCode,
                UserId = decryptedUserId,
                Hash = validationHash,
            };

            ValidationResult validationResult = await this.callbackBusiness.MakeCallbackAsync(callbackRequestDto, callbackUrl);

            if (validationResult.Errors.Count > 0)
            {
                return Problem(detail: validationResult.ToString(";"), statusCode: (int)HttpStatusCode.UnprocessableEntity);
            }

            return new InitResponseDto()
            {
                Success = true,
                Country = countryCode,
                UserId = decryptedUserId,
                Hash = validationHash,
            };
        }
    }
}