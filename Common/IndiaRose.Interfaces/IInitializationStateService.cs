using System;

namespace IndiaRose.Interfaces
{
	public interface IInitializationStateService
	{
		void InitializationFinished();

		void AddInitializedCallback(Action callback);
	}
}