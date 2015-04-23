using System;
using System.Threading.Tasks;

namespace IndiaRose.Framework
{
	public static class AsyncHelper
	{
		public static Task<T> CreateAsyncFromCallback<T>(Action<Action<T>> asyncStarter)
		{
			TaskCompletionSource<T> taskSource = new TaskCompletionSource<T>();
			Task<T> t = taskSource.Task;

			Task.Factory.StartNew(() =>
			{
				asyncStarter(taskSource.SetResult);
			});

			return t;
		}

		public static Task<TResult> CreateAsyncFromCallback<TAsyncResult, TResult>(Action<Action<TAsyncResult>> asyncStarter, Func<TAsyncResult, TResult> resultHandler)
		{
			TaskCompletionSource<TResult> taskSource = new TaskCompletionSource<TResult>();
			Task<TResult> t = taskSource.Task;

			Task.Factory.StartNew(() =>
			{
				asyncStarter(res => taskSource.SetResult(resultHandler(res)));
			});

			return t;
		}
	}
}
