namespace BackAlmancen.Application.Responses
{
    public class Response<T>
    {
        public Response(T data)
        {
            Data = data;
        }

        public Response()
        {

        }

        public T? Data { get; set; }
        public int StatusCode { get; set; } = 200;
        public string MessageError { get; set; } = "successful";
        public bool Success { get; set; } = true;
        public static Response<T> FromException(Exception ex, int statusCode)
        {
            var response = new Response<T>
            {
                Data = default(T),
                StatusCode = statusCode
            };


            if (ex.InnerException != null)
            {
                response.MessageError = ex.InnerException.Message;
            }
            else
            {
                response.MessageError = ex.Message;
            }

            return response;
        }

        public static Response<T> Fail(string message, int statusCode = 400)
        {
            return new Response<T>
            {
                Success = false,
                MessageError = message,
                StatusCode = statusCode,
                Data = default // Al ser genérico, devolvemos el default de T
            };
        }
    }
}
