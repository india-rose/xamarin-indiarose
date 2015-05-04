﻿#region Usings

using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class AddIndiagramViewModel : AbstractCollectionViewModel
	{
		private bool _isCategory;
		private Indiagram _currentIndiagram;
		private bool _editMode;
		private IndiagramContainer _indiagram;

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
		public ICommand SaveCommand { get; private set; }

		[NavigationParameter(Mode = NavigationParameterMode.Optional)]
		public IndiagramContainer Indiagram
		{
			get { return _indiagram; }
			set
			{
				if (SetProperty(ref _indiagram, value) && value != null)
				{
					if (Indiagram.Indiagram.IsCategory)
					{
						CurrentIndiagram = new Category(Indiagram.Indiagram);
						IsCategory = true;
					}
					else
					{
						CurrentIndiagram = new Indiagram(Indiagram.Indiagram);
					}
					EditMode = true;
				}
			}
		}

		public bool EditMode
		{
			get { return _editMode; }
			private set { SetProperty(ref _editMode, value); }
		}

		public bool IsCategory
		{
			get { return _isCategory; }
			set
			{
				if (CurrentIndiagram.HasChildren)
				{
					RaisePropertyChanged();
				}
				else
				{
					SetProperty(ref _isCategory, value);
				}
			}
		}

		public Indiagram CurrentIndiagram
		{
			get { return _currentIndiagram; }
			set { SetProperty(ref _currentIndiagram, value); }
		}

		public AddIndiagramViewModel()
		{
			ImageChoiceCommand = new DelegateCommand(ImageChoiceAction);
			SoundChoiceCommand = new DelegateCommand(SoundChoiceAction);
			RootCommand = new DelegateCommand(RootAction);
			ResetSoundCommand = new DelegateCommand(ResetSoundAction);
			ListenCommand = new DelegateCommand(ListenAction);
			ActivateCommand = new DelegateCommand(ActivateAction);
			DesactivateCommand = new DelegateCommand(DesactivateAction);
			CopyCommand = new DelegateCommand(CopyAction);
			PasteCommand = new DelegateCommand(PasteAction);
			SelectCategoryCommand = new DelegateCommand(SelectCategoryAction);
			SaveCommand = new DelegateCommand(SaveAction);

			CurrentIndiagram = new Indiagram();
		}

		protected void SelectCategoryAction()
		{
			NavigationService.Navigate(Views.ADMIN_COLLECTION_SELECTCATEGORY, new Dictionary<string, object>
			{
				{"Indiagram", CurrentIndiagram},
				{"ExcludedIndiagram", Indiagram.Indiagram}
			});
		}

		protected void ActivateAction()
		{
			CurrentIndiagram.IsEnabled = true;
		}

		protected void DesactivateAction()
		{
			CurrentIndiagram.IsEnabled = false;
		}

		protected void SaveAction()
		{
			if (string.IsNullOrWhiteSpace(CurrentIndiagram.Text))
			{
				PopupService.DisplayPopup(LocalizationService.GetString("Collection_MissingText", "Text"));
				return;
			}
			if (EditMode)
			{
				// edit indiagram in the storage
				if (Indiagram.Indiagram.IsCategory != IsCategory)
				{
					CollectionStorageService.Delete(Indiagram.Indiagram);
					Indiagram.Indiagram = IsCategory ? new Category(CurrentIndiagram, true) : new Indiagram(CurrentIndiagram, true);
					CollectionStorageService.Create(Indiagram.Indiagram);
				}
				else
				{
					Indiagram.Indiagram.Edit(CurrentIndiagram, true);
					CollectionStorageService.Update(Indiagram.Indiagram);
				}
			}
			else
			{
				//create a new indiagram in the storage
				var toAddIndiagram = IsCategory ? new Category(CurrentIndiagram, true) : new Indiagram(CurrentIndiagram, true);
				CollectionStorageService.Create(toAddIndiagram);
			}

			BackAction();
		}

		protected void ImageChoiceAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_IMAGECHOICE, new Dictionary<string, object>
			{
				{"Indiagram", CurrentIndiagram}
			});
		}

		protected void SoundChoiceAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_SOUNDCHOICE, new Dictionary<string, object>
			{
				{"Indiagram", CurrentIndiagram}
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
			if (string.IsNullOrWhiteSpace(CurrentIndiagram.Text) && !CurrentIndiagram.HasCustomSound)
			{
				PopupService.DisplayPopup(LocalizationService.GetString("Collection_MissingSound", "Text"));
			}
		}

		protected void CopyAction()
		{
			CopyPasteService.Copy(CurrentIndiagram, IsCategory);
		}

		protected void PasteAction()
		{
			CurrentIndiagram = CopyPasteService.Paste();
			IsCategory = CurrentIndiagram.IsCategory;
		}
	}
}