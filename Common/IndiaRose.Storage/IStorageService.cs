using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndiaRose.Storage
{
	public interface IStorageService
	{
		string DabatabasePath { get; }

		string RootPath { get; }

		Task InitializeAsync();
	}
}
