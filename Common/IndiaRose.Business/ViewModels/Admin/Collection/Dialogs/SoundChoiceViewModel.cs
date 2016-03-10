﻿#region Usings 

using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    /// <summary>
    /// VueModèle pour le dialogue de choix du son
    /// </summary>
	public class SoundChoiceViewModel : AbstractCollectionViewModel
	{
		private Indiagram _indiagramContainer;

		public IMediaService MediaService
		{
			get { return LazyResolver<IMediaService>.Service; }
		}

		public ICommand RecordSoundCommand { get; private set; }
		public ICommand GalleryCommand { get; private set; }

		[NavigationParameter]
		public Indiagram Indiagram
		{
			get { return _indiagramContainer; }
			set { SetProperty(ref _indiagramContainer, value); }
		}

		public SoundChoiceViewModel()
		{
			RecordSoundCommand = new DelegateCommand(RecordSoundAction);
			GalleryCommand = new DelegateCommand(GalleryAction);
		}

		private void RecordSoundAction()
		{
			MessageDialogService.DismissCurrentDialog();
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_RECORDSOUND, new Dictionary<string, object>
			{
				{"Indiagram", Indiagram}
			});
		}

		private async void GalleryAction()
		{
			Indiagram.SoundPath = await MediaService.GetSoundFromGalleryAsync();
			MessageDialogService.DismissCurrentDialog();
		}
	}
}