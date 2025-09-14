using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Enums;
using Fcg.Game.Infrastructure;

namespace Fcg.Game.Api.Setup
{
	public static class DataSeed
	{
		// Este método deve ser modificado para ser uma seed no banco de dados.
		public static void AddGames(this WebApplication _)
		{
			var c = new DatabaseGameContext();

			GameModel[] games = [new GameModel("Space Invaders", "Defend Earth from waves of aliens in this retro-style arcade shooter.", Genre.Action, new DateTime(2023, 1, 5), 100)
{
	Id = Guid.Parse("7776e5bc-1386-4776-8fd9-e5bdd5a7df04")
},
	new GameModel("Castle Quest", "Explore ancient castles and solve challenging puzzles to uncover lost secrets.", Genre.Adventure, new DateTime(2022, 5, 12), 150)
	{
		Id = Guid.Parse("1f8a2e69-c7e3-41f0-b14a-6df8bfe61a9d")
	},
	new GameModel("Mystery Maze", "Navigate an ever-changing maze while avoiding traps and monsters.", Genre.Puzzle, new DateTime(2021, 11, 20), 80)
	{
		Id = Guid.Parse("2b983e4e-60e1-4cbb-846a-421a3ff0deac")
	},
	new GameModel("Zombie Survival", "Survive wave after wave of the undead in this brutal action-horror experience.", Genre.Horror, new DateTime(2023, 3, 8), 120)
	{
		Id = Guid.Parse("f95f92aa-2e4f-4c7d-92a5-e97c6a889f38")
	},
	new GameModel("Ocean Explorer", "Dive into the depths and discover lost civilizations beneath the sea.", Genre.Adventure, new DateTime(2022, 7, 15), 130)
	{
		Id = Guid.Parse("5ddbe661-e6c5-4df6-93c2-69e8ac7295f9")
	},
	new GameModel("Sky Racer Game Sample", "Pilot high-speed jets in dangerous aerial races above exotic landscapes.", Genre.Puzzle, new DateTime(2023, 9, 1), 90)
	{
		Id = Guid.Parse("f2bda1bc-6df0-4e70-b3f2-7fbe31384d6b")
	},
	new GameModel("Jungle Adventure", "Survive in the wild and uncover ancient jungle temples.", Genre.Adventure, new DateTime(2022, 6, 6), 95)
	{
		Id = Guid.Parse("7bb46c33-fcf0-45f6-8ae7-3de7cf7e6120")
	},
	new GameModel("Alien Shooter", "Arm yourself and eliminate waves of alien threats in fast-paced missions.", Genre.Action, new DateTime(2021, 9, 17), 110)
	{
		Id = Guid.Parse("92cd7805-7e86-4643-a589-b85a7684d655")
	},
	new GameModel("Puzzle Land", "Solve intricate puzzles across beautiful, handcrafted worlds.", Genre.Puzzle, new DateTime(2023, 4, 25), 75)
	{
		Id = Guid.Parse("a8b98cb3-c50d-4643-afe4-13bd2c3a166f")
	},
	new GameModel("Dungeon Run", "Fight your way through procedurally generated dungeons in a race for loot.", Genre.Horror, new DateTime(2023, 2, 10), 115)
	{
		Id = Guid.Parse("cfbd2a3a-3d87-4e3e-b26e-3d428fe9cb0c")
	},
	new GameModel("Time Traveler", "Travel through time to alter history in this narrative-driven RPG.", Genre.RPG, new DateTime(2024, 1, 19), 140)
	{
		Id = Guid.Parse("acbd1f8f-dba3-4f9e-bfd2-5ed5a0e3e1e6")
	},
	new GameModel("Monster Defense", "Build towers and defend your base from waves of mythical monsters.", Genre.Strategy, new DateTime(2021, 12, 1), 85)
	{
		Id = Guid.Parse("e28b29e0-cfc1-446c-b3cc-9fe7693bb7c7")
	},
	new GameModel("Desert Storm", "Engage in high-octane vehicular combat across the harsh desert.", Genre.Action, new DateTime(2022, 3, 22), 100)
	{
		Id = Guid.Parse("a079e511-f4c5-4557-bfd8-c316f3d7f21a")
	},
	new GameModel("Robot Rebellion", "Crush the uprising of rogue machines before humanity falls.", Genre.RPG, new DateTime(2023, 10, 30), 125)
	{
		Id = Guid.Parse("777dbd12-df2e-4e38-9f08-924e43a3cd40")
	},
	new GameModel("Maze Escape", "Find your way out before time runs out in this fast-paced maze runner.", Genre.Puzzle, new DateTime(2023, 8, 14), 70)
	{
		Id = Guid.Parse("627f25b6-3f47-4fd1-b0cb-5c0b29ce640e")
	},
	new GameModel("Ice Climber", "Scale icy cliffs, battle frost creatures, and uncover frozen relics.", Genre.Strategy, new DateTime(2022, 1, 3), 105)
	{
		Id = Guid.Parse("3fd49c0c-3cc3-441f-9946-2b6b38ea017f")
	},
	new GameModel("Fire Fight", "Join the elite fire brigade in saving cities from blazing infernos.", Genre.Simulation, new DateTime(2024, 5, 5), 90)
	{
		Id = Guid.Parse("09c95f43-4241-493d-8c8d-01e62b0de4d0")
	}];

			c.Games.AddRange(games);

			c.SaveChanges();
		}
	}
}
