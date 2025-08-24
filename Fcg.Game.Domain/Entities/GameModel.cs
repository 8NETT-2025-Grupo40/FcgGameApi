using Fcg.Game.Domain.Base;
using Fcg.Game.Domain.Enums;
using Fcg.Game.Domain.Exceptions;
using Fcg.Game.Domain.ValueObjects;

namespace Fcg.Game.Domain.Entities;

public class GameModel : EntityBase
{
	protected GameModel() { }

	public GameModel(Title title, Description description, Genre genre, DateTime releaseDate, Price price)
	{
		if (genre is Genre.Unknown)
		{
			throw new DomainException("Genre must be specified.");
		}

		Title = title;
		Description = description;
		Genre = genre;
		ReleaseDate = releaseDate;
		Price = price;
		Status = StatusBase.Active;
	}

	public Title Title { get; private set; } = null!;
	public Description Description { get; private set; } = null!;
	public Genre Genre { get; private set; }
	public DateTime ReleaseDate { get; private set; }
	public Price Price { get; private set; } = null!;

	//public List<PlayerProfileGame> PlayerProfiles { get; private set; } = [];
}