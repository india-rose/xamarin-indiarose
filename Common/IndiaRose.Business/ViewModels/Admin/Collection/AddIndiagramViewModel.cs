using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using IndiaRose.Storage;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class AddIndiagramViewModel : AbstractSettingsViewModel
    {
        #region Service
        public IMessageDialogService MessageDialogService
        {
            get { return LazyResolver<IMessageDialogService>.Service; }
        }
        public ICollectionStorageService CollectionStorageService
        {
            get { return LazyResolver<ICollectionStorageService>.Service; }
        }
        public IPopupService PopupService
        {
            get { return LazyResolver<IPopupService>.Service; }
        }
        public ILocalizationService LocalizationService
        {
            get { return LazyResolver<ILocalizationService>.Service; }
        }
        public ICopyPasteService CopyPasteService
        {
            get { return LazyResolver<ICopyPasteService>.Service; }
        }
        #endregion

        #region Command
        public ICommand ImageChoiceCommand { get; private set; }
        public ICommand SoundChoiceCommand { get; private set; }
        public ICommand RootCommand { get; private set; }
        public ICommand ResetSoundCommand { get; private set; }
        public ICommand ListenCommand { get; private set; }
        public ICommand ActivateCommand { get; private set; }
        public ICommand DesactivateCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }
		public ICommand SelectCategoryCommand { get; private set; }


        #endregion

        #region Properties
        private string _bro1;
	    private IndiagramContainer _initialIndiagram;
        private IndiagramContainer _currentIndiagram;

        public string Bro1
        {
            get { return _bro1; }
            set { SetProperty(ref _bro1, value); }
        }
        public ObservableCollection<string> Brothers { get; private set; }

        private bool _categ ;
        public bool Categ
        {
            get { return _categ; }
            set
            {
                if (!CurrentIndiagram.Indiagram.HasChildren())
                {
                    SetProperty(ref _categ, value);
                }
                else
                {
                    RaisePropertyChanged();
                }
            }
        }

	    [NavigationParameter]
        protected IndiagramContainer InitialIndiagram
	    {
		    get {return _initialIndiagram;}
		    set
		    {
			    if (SetProperty(ref _initialIndiagram, value) && value != null)
			    {
					if (InitialIndiagram.Indiagram.IsCategory)
					{
						CurrentIndiagram.Indiagram = new Category(InitialIndiagram.Indiagram);
						Categ = true;
					}
					else
					{
						CurrentIndiagram.Indiagram = new Indiagram(InitialIndiagram.Indiagram);
					}
			    }
		    }
	    }
        public IndiagramContainer CurrentIndiagram
        {
            get { return _currentIndiagram ; }
            set { SetProperty(ref _currentIndiagram, value); }
        }
        #endregion

        public AddIndiagramViewModel()
        {
			SelectCategoryCommand = new DelegateCommand(SelectCategoryAction);
            ImageChoiceCommand = new DelegateCommand(ImageChoiceAction);
            SoundChoiceCommand = new DelegateCommand(SoundChoiceAction);
            RootCommand = new DelegateCommand(RootAction);
            ResetSoundCommand=new DelegateCommand(ResetSoundAction);
            ListenCommand=new DelegateCommand(ListenAction);
            ActivateCommand = new DelegateCommand(ActivateAction);
            DesactivateCommand = new DelegateCommand(DesactivateAction);
            CopyCommand = new DelegateCommand(CopyAction);
            PasteCommand = new DelegateCommand(PasteAction);

            CurrentIndiagram = new IndiagramContainer(new Indiagram());
        }

	    #region Action

	    protected void SelectCategoryAction()
	    {
			NavigationService.Navigate(Views.ADMIN_COLLECTION_SELECTCATEGORY, new Dictionary<string, object>()
             {
                 {"AddIndiagramContainer", CurrentIndiagram}
             });
	    }
        protected void ActivateAction()
        {
            CurrentIndiagram.Indiagram.IsEnabled = true;
        }
        protected void DesactivateAction()
        {
            CurrentIndiagram.Indiagram.IsEnabled = false;
        }
        protected override void SaveAction()
        {
            if (CurrentIndiagram.Indiagram.Text == null)
            {
                PopupService.AfficherPopup(LocalizationService.GetString("AIP_NoName","Text"));
                return;
            }
            if (InitialIndiagram == null)
            {
                //creation d'un indi
                Indiagram toAddIndiagram;
                toAddIndiagram = Categ ? new Category(CurrentIndiagram.Indiagram,true) : new Indiagram(CurrentIndiagram.Indiagram,true);
                CollectionStorageService.Create(toAddIndiagram);
            }
            else
            {
                //edition d'un indi
                if (InitialIndiagram.Indiagram.IsCategory && !Categ)
                {
                    CollectionStorageService.Delete(InitialIndiagram.Indiagram);
                    InitialIndiagram.Indiagram = new Indiagram(CurrentIndiagram.Indiagram,true);
                    CollectionStorageService.Create(InitialIndiagram.Indiagram);

                }
                else if (!InitialIndiagram.Indiagram.IsCategory && Categ)
                {
                    CollectionStorageService.Delete(InitialIndiagram.Indiagram);
                    InitialIndiagram.Indiagram = new Category(CurrentIndiagram.Indiagram,true);
                    CollectionStorageService.Create(InitialIndiagram.Indiagram);
                }
                else
                {
                    InitialIndiagram.Indiagram.Edit(CurrentIndiagram.Indiagram,true);
                    CollectionStorageService.Update(InitialIndiagram.Indiagram);
                }

            }

            BackAction();
        }
        protected void ImageChoiceAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_IMAGECHOICE,new Dictionary<string, object>
            {
                {"Indiagram",CurrentIndiagram}
            });
        }
        protected void SoundChoiceAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_SOUNDCHOICE, new Dictionary<string, object>
            {
                {"Indiagram",CurrentIndiagram}
            });
        }
        protected void RootAction()
        {
            CurrentIndiagram.Indiagram.Parent = null;
        }
        protected void ResetSoundAction()
        {
            CurrentIndiagram.Indiagram.SoundPath = null;
        }
        protected void ListenAction()
        {
            if (CurrentIndiagram.Indiagram.Text == null)
            {
                PopupService.AfficherPopup(LocalizationService.GetString("AIP_NoSoundError", "Text"));
            }
        }
        protected void CopyAction()
        {
            CopyPasteService.Copy(CurrentIndiagram.Indiagram,Categ);
        }
        protected void PasteAction()
        {
            CurrentIndiagram.Indiagram = CopyPasteService.Paste();
            Categ = CurrentIndiagram.Indiagram.IsCategory;
        }

        #endregion
    }
}
