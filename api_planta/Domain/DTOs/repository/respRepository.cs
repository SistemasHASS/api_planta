public class ApiResponse<T>
{
    public bool Error { get; set; }
    public T? Data { get; set; }
    public string? Mensaje { get; set; }
}