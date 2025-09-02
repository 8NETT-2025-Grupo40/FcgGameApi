using Fcg.Game.Domain.Exceptions;

namespace Fcg.Game.Domain.ValueObjects;

public record Nickname
{
	const int MINIMUM_LENGTH = 3;
	const int MAXIMUM_LENGTH = 20;

	public string Value { get; } = null!;
	protected Nickname() { }

	public Nickname(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new DomainException("Nickname is required.");
		}

		if (value.Length is < MINIMUM_LENGTH or > MAXIMUM_LENGTH)
		{
			throw new DomainException("Nickname must be between 3 and 20 characters long.");
		}

		// Validação de caracteres pode ser incluída
		Value = value.Trim();
	}

	public override string ToString() => Value;

	public static implicit operator Nickname(string value) => new Nickname(value);
}