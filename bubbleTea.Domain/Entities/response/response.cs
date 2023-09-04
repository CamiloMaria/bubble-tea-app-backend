namespace BubbleTea.Domain.Entities
{
    public class Response<T>
    {
        public bool Success { get; set; } = true; 
        public int StatusCode { get; set; } = 200; 
        public string ReasonPhrase { get; set; } = "OK"; 
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        // Constructor para crear una respuesta exitosa con datos
        public Response(T? data)
        {
            Data = data;
        }

        // Constructor para crear una respuesta con errores
        public Response(string message, int statusCode = 500, string reasonPhrase = "Internal Server Error")
        {
            Success = false;
            Message = message;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
        }
    }
}
