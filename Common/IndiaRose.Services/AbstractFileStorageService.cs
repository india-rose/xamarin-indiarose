using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Newtonsoft.Json;
using PCLStorage;
using Storm.Mvvm;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Services
{
	public class AbstractFileStorageService : NotifierBase
	{
		protected string FolderPath { get; set; }
		protected string FileName { get; set; }

		#region Services

		protected IStorageService StorageService
		{
			get { return LazyResolver<IStorageService>.Service; }
		}

		protected ILoggerService LoggerService
		{
			get { return LazyResolver<ILoggerService>.Service; }
		}

		#endregion
		
		protected async Task<bool> ExistsOnDiskAsync()
		{
			try
			{
				IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(FolderPath);
				ExistenceCheckResult result = await folder.CheckExistsAsync(FileName);
				return result == ExistenceCheckResult.FileExists;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		protected async Task<string> LoadFromDiskAsync()
		{
			try
			{
				IFile file = await FileSystem.Current.GetFileFromPathAsync(Path.Combine(FolderPath, FileName));
				return await file.ReadAllTextAsync();
			}
			catch (Exception e)
			{
				LoggerService.Log(string.Format("IndiaRose.Services.AbstractFileStorageService({0}).LoadFromDiskAsync() : exception while trying to load content from file : {1}", FileName, e), MessageSeverity.Critical);
				return null;
			}

		}

		protected async Task SaveToDiskAsync(string content)
		{
			try
			{
				IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(FolderPath);
				IFile file = await folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
				await file.WriteAllTextAsync(content);
			}
			catch (Exception e)
			{
				LoggerService.Log(string.Format("IndiaRose.Services.AbstractFileStorageService({0}).SaveToDiskAsync() : exception while trying to write content to file : {1}", FileName, e), MessageSeverity.Critical);
			}
		}
	}
}
