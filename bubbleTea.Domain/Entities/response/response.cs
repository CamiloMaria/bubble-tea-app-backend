namespace BubbleTea.Domain.Entities
{
    public class Response<T>
    {
        public bool Success { get; set; } = true;
        public int StatusCode { get; set; } = 200;
        public string ReasonPhrase { get; set; } = "OK";
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<string> Errors { get; set; } = new List<string>();


        // Parameterless constructor
        public Response()
        {
        }

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

        // Método para agregar un mensaje de error a la colección de errores
        public void AddError(string error)
        {
            Success = false;
            Errors.Add(error);
        }
    }
}
