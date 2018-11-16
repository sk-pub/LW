using System.Collections.ObjectModel;

namespace Linnworks.Api
{
	public class QueryResult
	{
		public ReadOnlyCollection<ReadOnlyDictionary<string, string>> Results { get; set; }
	}
}
