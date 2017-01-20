using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndiaRose.Core.Interfaces
{
	public interface ILoggerService
	{
		void Debug(string message);

		void Information(string message);

		void Warning(string message);

		void Error(string message);

		void Exception(Exception ex);
	}
}
