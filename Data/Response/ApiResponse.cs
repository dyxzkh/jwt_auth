namespace asp.net_jwt.Data.Response
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Details { get; set; }

        public ApiResponse(int statusCode = 500, string? message = null, object? details = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            Details = details;
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            #pragma warning disable CS8603 // Possible null reference return.
            return statusCode switch
            {
                400 => "Bad Request.",
                401 => "Unauthorized.",
                403 => "Forbidden.",
                404 => "Resource not found.",
                500 => "An internal server error occurred.",
                _ => null
            };
            #pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
