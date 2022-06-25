namespace BasicCoreApi.Errors
{
    public static class ApiErrorMessage
    {
        public const string CallbackValidatorEmpty = "Callback Validator value came empty.";

        public const string CallbackStatusIsFalse = "Callback status came as false.";

        public const string CallbackIsNull = "Callback is null.";

        public const string MissingCountryCookie = "'country' cookie is missing from the request.";

        public const string CorrectAutorizationHeaderFormat = "Correct Autorization header format: \"AES {base64 encypted user id}\".";
    }
}
