using Fcg.Game.Domain.Exceptions;

namespace Fcg.Game.Domain.ValueObjects;

public record Title
{
	const int MINIMUM_LENGTH = 10;
	const int MAXIMUM_LENGTH = 255;

	public string Value { get; private set; }

	protected Title() => Value = string.Empty;

	public Title(string title)
	{
		if (string.IsNullOrWhiteSpace(title))
		{
			throw new DomainException("The title must not be empty");
		}

		if (title.Length < MINIMUM_LENGTH || title.Length > MAXIMUM_LENGTH)
		{
			throw new DomainException($"The title length must be between {MINIMUM_LENGTH} and {MAXIMUM_LENGTH} characters");
		}

		Value = title.Trim();
	}

	public override string ToString() => Value;

	public static implicit operator Title(string title) => new(title);
	public static implicit operator string(Title title) => title.ToString();
}