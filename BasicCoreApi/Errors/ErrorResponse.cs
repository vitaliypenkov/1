namespace BasicCoreApi.Errors
{
    public class ErrorResponse
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public ErrorResponse(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public override string ToString()
        {
            return $"ErrorCode:{ErrorCode} Message:{Message}.";
        }
    }
}
