using System.Collections.Generic;
using System.Threading.Tasks;

namespace IndiaRose.Core.Interfaces
{
	public interface IFontService
	{
		Task<List<string>> GetFontDisplayNames();

		Task<string> GetFontFamilyForDisplayName(string name);
	}
}
