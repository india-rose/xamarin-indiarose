﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
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
        //TODO A TESTE LES CATEGORIES
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
        public ICommand ChooseCategoryCommand { get; private set; }
        public ICommand ActivateCommand { get; private set; }
        public ICommand DesactivateCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }


        #endregion

        #region Properties
        private string _bro1;
	    private Indiagram _initialIndiagram;
        private Indiagram _currentIndiagram;

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
                if (!CurrentIndiagram.HasChildren())
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
	    protected Indiagram InitialIndiagram
	    {
		    get {return _initialIndiagram;}
		    set
		    {
			    if (SetProperty(ref _initialIndiagram, value) && value != null)
			    {
					if (InitialIndiagram.IsCategory)
					{
						CurrentIndiagram = new Category(InitialIndiagram);
						Categ = true;
					}
					else
					{
						CurrentIndiagram = new Indiagram(InitialIndiagram);
					}
			    }
		    }
	    }

        public Indiagram CurrentIndiagram
        {
            get { return _currentIndiagram ; }
            set { SetProperty(ref _currentIndiagram, value); }
        }
        #endregion

        public AddIndiagramViewModel()
        {
            ImageChoiceCommand = new DelegateCommand(ImageChoiceAction);
            SoundChoiceCommand = new DelegateCommand(SoundChoiceAction);
            RootCommand = new DelegateCommand(RootAction);
            ResetSoundCommand=new DelegateCommand(ResetSoundAction);
            ListenCommand=new DelegateCommand(ListenAction);
            ChooseCategoryCommand=new DelegateCommand(ChooseCategoryAction);
            ActivateCommand = new DelegateCommand(ActivateAction);
            DesactivateCommand = new DelegateCommand(DesactivateAction);
            CopyCommand = new DelegateCommand(CopyAction);
            PasteCommand = new DelegateCommand(PasteAction);

			CurrentIndiagram = new Indiagram();
        }

	    #region Action

        protected void ActivateAction()
        {
            CurrentIndiagram.IsEnabled = true;
        }
        protected void DesactivateAction()
        {
            CurrentIndiagram.IsEnabled = false;
        }
        protected void ChooseCategoryAction()
        {
            //TODO a faire avec le frag
        }
        protected override void SaveAction()
        {
            if (CurrentIndiagram.Text == null)
            {
                PopupService.AfficherPopup(LocalizationService.GetString("AIP_NoName","Text"));
                return;
            }
            if (InitialIndiagram == null)
            {
                //creation d'un indi
                InitialIndiagram = Categ ? new Category(CurrentIndiagram) : CurrentIndiagram;
                CollectionStorageService.Create(InitialIndiagram);
            }
            else
            {
                //edition d'un indi
                if (InitialIndiagram.IsCategory && !Categ)
                {
                    CollectionStorageService.Delete(InitialIndiagram);
                    InitialIndiagram = new Indiagram(CurrentIndiagram);
                    CollectionStorageService.Create(InitialIndiagram);

                }
                else if (!InitialIndiagram.IsCategory && Categ)
                {
                    CollectionStorageService.Delete(InitialIndiagram);
                    InitialIndiagram = new Category(CurrentIndiagram);
                    CollectionStorageService.Create(InitialIndiagram);
                }
                else
                {
                    InitialIndiagram.Edit(CurrentIndiagram);
                    CollectionStorageService.Update(InitialIndiagram);
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
            CurrentIndiagram.Parent = null;
        }
        protected void ResetSoundAction()
        {
            CurrentIndiagram.SoundPath = null;
        }
        protected void ListenAction()
        {
            if (CurrentIndiagram.Text == null)
            {
                PopupService.AfficherPopup(LocalizationService.GetString("AIP_NoSoundError", "Text"));
            }
            
        }
        protected void CopyAction()
        {
            CopyPasteService.Copy(CurrentIndiagram,Categ);
        }
        protected void PasteAction()
        {
            CurrentIndiagram = CopyPasteService.Paste();
            Categ = CurrentIndiagram.IsCategory;
            RaisePropertyChanged();
        }

        #endregion
    }
}
