namespace Fcg.Game.Domain.Exceptions
{
	internal class DomainException : Exception
	{
		public DomainException()
		{
		}

		public DomainException(string? message) : base(message)
		{
		}

		public DomainException(string? message, Exception? innerException) : base(message, innerException)
		{
		}
	}
}