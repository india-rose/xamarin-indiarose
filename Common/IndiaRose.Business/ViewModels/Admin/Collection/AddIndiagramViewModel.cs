#region Usings 

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
		private string _bro1;
		private bool _categ;
		private IndiagramContainer _currentIndiagram;
		private bool _editMode;
		private IndiagramContainer _initialIndiagram;

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

		[NavigationParameter]
		public bool EditMode
		{
			get { return _editMode; }
			set { SetProperty(ref _editMode, value); }
		}

		public string Bro1
		{
			get { return _bro1; }
			set { SetProperty(ref _bro1, value); }
		}

		public List<Indiagram> Brothers { get; private set; }

		public bool Categ
		{
			get { return _categ; }
			set
			{
				if (!CurrentIndiagram.Indiagram.HasChildren)
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
			get { return _initialIndiagram; }
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
			get { return _currentIndiagram; }
			set { SetProperty(ref _currentIndiagram, value); }
		}

		public AddIndiagramViewModel()
		{
			SelectCategoryCommand = new DelegateCommand(SelectCategoryAction);
			ImageChoiceCommand = new DelegateCommand(ImageChoiceAction);
			SoundChoiceCommand = new DelegateCommand(SoundChoiceAction);
			RootCommand = new DelegateCommand(RootAction);
			ResetSoundCommand = new DelegateCommand(ResetSoundAction);
			ListenCommand = new DelegateCommand(ListenAction);
			ActivateCommand = new DelegateCommand(ActivateAction);
			DesactivateCommand = new DelegateCommand(DesactivateAction);
			CopyCommand = new DelegateCommand(CopyAction);
			PasteCommand = new DelegateCommand(PasteAction);
			SaveCommand = new DelegateCommand(SaveAction);

			CurrentIndiagram = new IndiagramContainer(new Indiagram());
			/*try
            {
                Brothers = CurrentIndiagram.Indiagram.Parent.Children;
                Brothers.Add(new Indiagram("Fin", null));
                Bro1 = Brothers[CurrentIndiagram.Indiagram.Position].Text;
            }
            catch (NullReferenceException)
            {
                Brothers = CollectionStorageService.GetTopLevel();
                Brothers.Add(new Indiagram("Fin",null));
                Bro1 = Brothers[Brothers.Count-1].Text;
            }*/
			//pourquoi il veut pas avec les catégories ??
		}

		protected void SelectCategoryAction()
		{
			NavigationService.Navigate(Views.ADMIN_COLLECTION_SELECTCATEGORY, new Dictionary<string, object>
			{
				{"Indiagram", CurrentIndiagram}
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

		protected void SaveAction()
		{
			if (CurrentIndiagram.Indiagram.Text == null)
			{
				PopupService.DisplayPopup(LocalizationService.GetString("Collection_MissingText", "Text"));
				return;
			}
			if (InitialIndiagram == null)
			{
				//creation d'un indi
				var toAddIndiagram = Categ ? new Category(CurrentIndiagram.Indiagram, true) : new Indiagram(CurrentIndiagram.Indiagram, true);
				CollectionStorageService.Create(toAddIndiagram);
			}
			else
			{
				//edition d'un indi
				if (InitialIndiagram.Indiagram.IsCategory && !Categ)
				{
					CollectionStorageService.Delete(InitialIndiagram.Indiagram);
					InitialIndiagram.Indiagram = new Indiagram(CurrentIndiagram.Indiagram, true);
					CollectionStorageService.Create(InitialIndiagram.Indiagram);
				}
				else if (!InitialIndiagram.Indiagram.IsCategory && Categ)
				{
					CollectionStorageService.Delete(InitialIndiagram.Indiagram);
					InitialIndiagram.Indiagram = new Category(CurrentIndiagram.Indiagram, true);
					CollectionStorageService.Create(InitialIndiagram.Indiagram);
				}
				else
				{
					InitialIndiagram.Indiagram.Edit(CurrentIndiagram.Indiagram, true);
					CollectionStorageService.Update(InitialIndiagram.Indiagram);
				}
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
				PopupService.DisplayPopup(LocalizationService.GetString("Collection_MissingSound", "Text"));
			}
		}

		protected void CopyAction()
		{
			CopyPasteService.Copy(CurrentIndiagram.Indiagram, Categ);
		}

		protected void PasteAction()
		{
			CurrentIndiagram.Indiagram = CopyPasteService.Paste();
			Categ = CurrentIndiagram.Indiagram.IsCategory;
		}
	}
}