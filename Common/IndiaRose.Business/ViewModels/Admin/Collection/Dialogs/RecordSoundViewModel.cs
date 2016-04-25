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
    /// VueModèle pour l'enregistrement d'un son
    /// </summary>
	public class RecordSoundViewModel : AbstractCollectionViewModel
	{
		private Indiagram _indiagramContainer;
		private bool _isRecording;

		public IMediaService MediaService => LazyResolver<IMediaService>.Service;

        public ICommand CloseCommand { get; private set; }
		public ICommand RecordCommand { get; private set; }
		public ICommand StopCommand { get; private set; }

		[NavigationParameter]
		public Indiagram Indiagram
		{
			get { return _indiagramContainer; }
			set { SetProperty(ref _indiagramContainer, value); }
		}

		public bool IsRecording
		{
			get { return _isRecording; }
			set { SetProperty(ref _isRecording, value); }
		}

		public RecordSoundViewModel()
		{
			CloseCommand = new DelegateCommand(CloseAction);
			RecordCommand = new DelegateCommand(RecordAction);
			StopCommand = new DelegateCommand(StopAction);
		}

        /// <summary>
        /// Ferme le dialogue
        /// </summary>
		protected void CloseAction()
		{
			MessageDialogService.DismissCurrentDialog();
		}

        /// <summary>
        /// Lance l'enregistrement
        /// </summary>
		protected void RecordAction()
		{
			IsRecording = true;
			MediaService.RecordSound();
		}

        /// <summary>
        /// Stop l'enregistrement et ferme le dialogue
        /// </summary>
		protected async void StopAction()
		{
			Indiagram.SoundPath = await MediaService.StopRecord();
			IsRecording = false;
			MessageDialogService.DismissCurrentDialog();
		}
	}
}