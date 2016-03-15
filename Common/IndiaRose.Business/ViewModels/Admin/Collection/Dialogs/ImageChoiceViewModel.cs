#region Usings 

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
    /// VueModèle du menu de choix de l'image d'un Indiagram
    /// </summary>
	public class ImageChoiceViewModel : AbstractCollectionViewModel
	{
		[NavigationParameter]
		public Indiagram Indiagram { get; set; }

		public ICommand CameraCommand { get; private set; }
		public ICommand GalleryCommand { get; private set; }

		public IMediaService MediaService
		{
			get { return LazyResolver<IMediaService>.Service; }
		}

		public ImageChoiceViewModel()
		{
			CameraCommand = new DelegateCommand(CameraAction);
			GalleryCommand = new DelegateCommand(GalleryAction);
		}

		public async void CameraAction()
		{
			SaveImageAction(await MediaService.GetPictureFromCameraAsync());
		}

		public async void GalleryAction()
		{
			SaveImageAction(await MediaService.GetPictureFromGalleryAsync());
		}

		private void SaveImageAction(string path)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				Indiagram.ImagePath = path;
			}
			MessageDialogService.DismissCurrentDialog();
		}
	}
}