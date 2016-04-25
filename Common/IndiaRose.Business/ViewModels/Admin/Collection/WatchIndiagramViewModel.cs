#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    /// <summary>
    /// VueModèle de la page de résumé d'un Indiagram
    /// </summary>
    public class WatchIndiagramViewModel : AbstractCollectionViewModel
    {
        private ITextToSpeechService TtsService
        {
            get { return LazyResolver<ITextToSpeechService>.Service; }
        }
        private IndiagramContainer _indiagramContainer;

        #region Commands
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ListenCommand { get; private set; }
        #endregion

        [NavigationParameter]
        public Indiagram Indiagram
        {
            set
            {
                if (value != null)
                {
                    IndiagramContainer = new IndiagramContainer(value);
                }
            }
        }

        public IndiagramContainer IndiagramContainer
        {
            get { return _indiagramContainer; }
            set { SetProperty(ref _indiagramContainer, value); }
        }

        public WatchIndiagramViewModel()
        {
            EditCommand = new DelegateCommand(EditAction);
            DeleteCommand = new DelegateCommand(DeleteAction);
            ListenCommand = new DelegateCommand(ListenAction);
        }

        /// <summary>
        /// Ouvre la page d'édition d'un Indiagram en lui passant l'IndiagramContainer
        /// </summary>
        private void EditAction()
        {
            NavigationService.Navigate(Views.ADMIN_COLLECTION_ADDINDIAGRAM, new Dictionary<string, object>
			{
				{"Indiagram", IndiagramContainer}
			});
        }

        /// <summary>
        /// Supprime l'Indiagram courant en demandant confirmation
        /// </summary>
        protected void DeleteAction()
        {
            if (IndiagramContainer == null || IndiagramContainer.Indiagram == null)
            {
                return;
            }

            if (IndiagramContainer.Indiagram.IsCategory)
            {
                MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETEWARNING_CATEGORY, new Dictionary<string, object>
				{
					{"Indiagram", IndiagramContainer.Indiagram},
					{"DeleteCallback", (Action)OnDeleted}
				});
            }
            else
            {
                MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETEWARNING_INDIAGRAM, new Dictionary<string, object>
				{
					{"Indiagram", IndiagramContainer.Indiagram},
					{"DeleteCallback", (Action)OnDeleted}
				});
            }
        }

        /// <summary>
        /// Lorsque l'Indiagram est supprimé on retourne à la page d'affichage de la collection
        /// </summary>
        private void OnDeleted()
        {
            NavigationService.GoBack();
        }

        /// <summary>
        /// Lecture de l'Indiagram
        /// </summary>
        private void ListenAction()
        {
            if (IndiagramContainer == null) return;
            TtsService.PlayIndiagram(IndiagramContainer.Indiagram);
        }
    }
}