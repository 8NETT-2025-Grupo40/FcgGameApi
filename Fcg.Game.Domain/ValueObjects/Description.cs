using Fcg.Game.Domain.Exceptions;

namespace Fcg.Game.Domain.ValueObjects;

public record Description
{
	private const int MINIMUM_LENGTH = 10;
	private const int MAXIMUM_LENGTH = 510;

	protected Description() => Value = string.Empty;

	public Description(string description)
	{
		if (string.IsNullOrWhiteSpace(description))
		{
			throw new DomainException("The description must not be empty");
		}

		if (description.Length <= MINIMUM_LENGTH || description.Length > MAXIMUM_LENGTH)
		{
			throw new DomainException($"The description length must be between {MINIMUM_LENGTH} and {MAXIMUM_LENGTH} characters");
		}

		Value = description.Trim();
	}

	public string Value { get; private set; }

	public override string ToString() => Value;

	public static implicit operator Description(string description) => new(description);
	public static implicit operator string(Description description) => description.ToString();
}