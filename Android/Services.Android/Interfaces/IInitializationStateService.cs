using System;

namespace IndiaRose.Services.Android.Interfaces
{
	public interface IInitializationStateService
	{
		void InitializationFinished();

		void AddInitializedCallback(Action callback);
	}
}