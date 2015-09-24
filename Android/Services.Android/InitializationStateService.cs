using System;
using System.Collections.Generic;
using IndiaRose.Services.Android.Interfaces;

namespace IndiaRose.Services.Android
{
	public class InitializationStateService : IInitializationStateService
	{
		private readonly object _mutex = new object();
		private readonly List<Action> _callbacks = new List<Action>();
		private bool _initialized;

		public void InitializationFinished()
		{
			lock (_mutex)
			{
				_initialized = true;

				foreach (Action callback in _callbacks)
				{
					callback();
				}
			}
		}

		public void AddInitializedCallback(Action callback)
		{
			lock (_mutex)
			{
				if (_initialized)
				{
					callback();
				}
				else
				{
					_callbacks.Add(callback);
				}
			}
		}
	}
}