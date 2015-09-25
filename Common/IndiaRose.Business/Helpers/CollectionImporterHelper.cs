using System.Collections.Generic;
using System.Threading.Tasks;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.Helpers
{
	public static class CollectionImporterHelper
	{
		private static bool _initialized;

		public static async Task ImportCollectionAsync()
		{
			ICollectionStorageService collectionStorageService = LazyResolver<ICollectionStorageService>.Service;
			ILoggerService loggerService = LazyResolver<ILoggerService>.Service;
			IDispatcherService dispatcherService = LazyResolver<IDispatcherService>.Service;
			IXmlService xmlService = LazyResolver<IXmlService>.Service;
			IMessageDialogService messageDialogService = LazyResolver<IMessageDialogService>.Service;
			IResourceService resourceService = LazyResolver<IResourceService>.Service;

			if (_initialized)
			{
				return;
			}
			_initialized = true;

			if (collectionStorageService.Collection.Count == 0)
			{
				if (await xmlService.HasOldCollectionFormatAsync())
				{
					dispatcherService.InvokeOnUIThread(() =>
						messageDialogService.Show(Dialogs.IMPORTING_COLLECTION, new Dictionary<string, object>
						{
							{"MessageUid", "ImportCollection_FromOldFormat"}
						}));

					loggerService.Log("==> Importing collection from old format");
					await xmlService.InitializeCollectionFromOldFormatAsync();
					loggerService.Log("# Import finished");
				}
				else
				{
					dispatcherService.InvokeOnUIThread(() =>
						messageDialogService.Show(Dialogs.IMPORTING_COLLECTION, new Dictionary<string, object>
						{
							{"MessageUid", "ImportCollection_FromZip"}
						}));

					loggerService.Log("==> Importing collection from zip file");
					await xmlService.InitializeCollectionFromZipStreamAsync(await resourceService.OpenZip("indiagrams.zip"));
				}
				loggerService.Log("# Import finished");
				messageDialogService.DismissCurrentDialog();
			}
		}
	}
}
