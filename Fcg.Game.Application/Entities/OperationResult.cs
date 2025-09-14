namespace Fcg.Game.Application.Entities;

public class OperationResult
{
	protected OperationResult(string message, bool isSuccessful)
	{
		Message = message;
		IsSuccessful = isSuccessful;
	}

	public string Message { get; protected set; }
	public bool IsSuccessful { get; protected set; }

	public static OperationResult CreateSuccessfulResponse() => new("Success", true);
	public static OperationResult CreateErrorResponse(string errorMessage) => new(errorMessage, false);
}

public class OperationResult<T> : OperationResult where T : class
{
	private OperationResult(string message, bool isSuccessful, T value = default) : base(message, isSuccessful)
	{
		Value = value;
		Message = message;
		IsSuccessful = isSuccessful;
	}

	public T Value { get; private set; }

	public static OperationResult<T> CreateSuccessfulResponse(T value) => new("Success", true, value);
	public static new OperationResult<T> CreateErrorResponse(string errorMessage) => new(errorMessage, false);
}