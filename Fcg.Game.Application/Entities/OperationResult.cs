namespace Fcg.Game.Application.Entities;

public class OperationResult<T> where T : class
{
	private OperationResult(string message, bool isSuccessful, T value = default)
	{
		Value = value;
		Message = message;
		IsSuccessful = isSuccessful;
	}

	public T Value { get; private set; }
	public string Message { get; private set; }
	public bool IsSuccessful { get; private set; }

	public static OperationResult<T> CreateSucessfulResponse(T value) => new OperationResult<T>("Success", true, value);
	public static OperationResult<T> CreateErrorResponse(string errorMessage) => new OperationResult<T>(errorMessage, false);
}
