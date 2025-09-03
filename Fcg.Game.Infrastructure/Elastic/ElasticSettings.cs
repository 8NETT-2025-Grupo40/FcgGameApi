using Fcg.Game.Application.Elastic;

namespace Fcg.Game.Infrastructure.Elastic
{
	public class ElasticSettings : IElasticSettings
	{
		public string Address { get; set; } = string.Empty;
	}
}
