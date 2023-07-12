using System.Net;

namespace MyProject.ViewModels;

public class ApiResponse<T>
{
    public DateTime ServerDatetime { get; set; } = DateTime.Now;
    public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
    public string? Message { get; set; }
    public T? Data { get; set; }
}

public class ApiBadRequestResponse<T> : ApiResponse<T>
{
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public ApiBadRequestResponse(string message)
    {
        StatusCode = (int)HttpStatusCode.BadRequest;
        if (!string.IsNullOrEmpty(message))
        {
            Message += (message != "" ? "\r\n" : "") + message;
        }
    }
}
public class ApiResponseServer<T> : ApiResponse<T>
{
    public string? Message { get; set; } 
    public ApiResponseServer(int statusCode, string message)
    {
        StatusCode = statusCode;
        if (!string.IsNullOrEmpty(message))
        {
            Message = message;
        }
    }
}

