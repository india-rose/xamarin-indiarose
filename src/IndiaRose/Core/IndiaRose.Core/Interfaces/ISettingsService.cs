using System.Threading;
using System.Threading.Tasks;
using IndiaRose.Core.Models;

namespace IndiaRose.Core.Interfaces
{
	public interface ISettingsService
	{
		Task<Settings> Load(CancellationToken ct);

		Task<Settings> Load();

		Task<bool> Save(Settings settings);

		Task<bool> Save(Settings settings, CancellationToken ct);
	}
}
