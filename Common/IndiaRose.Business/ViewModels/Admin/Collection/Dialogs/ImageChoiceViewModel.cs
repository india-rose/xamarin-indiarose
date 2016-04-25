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

		public IMediaService MediaService => LazyResolver<IMediaService>.Service;

        public ImageChoiceViewModel()
		{
			CameraCommand = new DelegateCommand(CameraAction);
			GalleryCommand = new DelegateCommand(GalleryAction);
		}

        /// <summary>
        /// Ouvre la camera pour que l'utilisateur prenne une photo
        /// </summary>
		public async void CameraAction()
		{
			SaveImageAction(await MediaService.GetPictureFromCameraAsync());
		}

        /// <summary>
        /// Ouvre la gallerie pour que l'utilisateur choississe une image
        /// </summary>
		public async void GalleryAction()
		{
			SaveImageAction(await MediaService.GetPictureFromGalleryAsync());
		}

        /// <summary>
        /// Associe le chemin de l'image choisi à l'Indiagram et ferme le dialogue courant
        /// </summary>
        /// <param name="path">Le chemin de l'image à associé à l'Indiagram</param>
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