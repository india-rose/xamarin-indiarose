using System;
using System.Collections.Generic;
using IndiaRose.Interfaces;

namespace IndiaRose.Services
{
	public class InitializationStateService : IInitializationStateService
	{
		private readonly object _mutex = new object();
		private readonly List<Action> _callbacks = new List<Action>();
		private readonly int _initializationCount;
		private int _currentCount;
		private bool _initialized;

		public InitializationStateService(int initializationCount)
		{
			_initializationCount = initializationCount;
		}

		public void InitializationFinished()
		{
			lock (_mutex)
			{
				_currentCount++;

				if (_currentCount == _initializationCount)
				{
					_initialized = true;

					foreach (Action callback in _callbacks)
					{
						callback();
					}
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