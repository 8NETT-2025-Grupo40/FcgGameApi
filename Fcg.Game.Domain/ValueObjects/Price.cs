using Fcg.Game.Domain.Exceptions;
using System.Globalization;

namespace Fcg.Game.Domain.ValueObjects;

public record Price
{
	public decimal Value { get; }

	protected Price() => Value = 0.0m;

	public Price(decimal value)
	{
		if (value < 0)
		{
			throw new DomainException("Price cannot be negative.");
		}

		Value = decimal.Round(value, 2);
	}

	public override string ToString() =>
		// Usa o formato inglês (EUA) de moeda ($1,234.56)
		Value.ToString("C2", CultureInfo.GetCultureInfo("en-US"));

	public static implicit operator Price(decimal value) => new(value);
}