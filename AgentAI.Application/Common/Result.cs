namespace AgentAI.Application.Common;

public class Result
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }

    public static Result SuccessResult() => new() { Success = true };
    public static Result SuccessResult(string message) => new() { Success = true, Message = message };
    public static Result FailureResult(string error) => new() { Success = false, Errors = new List<string> { error } };
    public static Result FailureResult(List<string> errors) => new() { Success = false, Errors = errors };
}

public class Result<T> : Result
{
    public T? Data { get; set; }

    public static Result<T> SuccessResult(T data) => new() { Success = true, Data = data };
    public static Result<T> SuccessResult(T data, string message) => new() { Success = true, Data = data, Message = message };
    public new static Result<T> FailureResult(string error) => new() { Success = false, Errors = new List<string> { error } };
    public new static Result<T> FailureResult(List<string> errors) => new() { Success = false, Errors = errors };
}
