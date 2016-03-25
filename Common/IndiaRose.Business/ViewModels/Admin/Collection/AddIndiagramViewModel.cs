#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// VueModèle de la page d'ajout/édition d'un Indiagram
    /// </summary>
    public class AddIndiagramViewModel : AbstractCollectionViewModel
    {
        private bool _isCategory;
        private Indiagram _currentIndiagram;
        private bool _editMode;
        private IndiagramContainer _indiagram;

        private List<Indiagram> _brothers;
        private Indiagram _beforeIndiagram;

        #region Services
        public IPopupService PopupService
        {
            get { return LazyResolver<IPopupService>.Service; }
        }

        public ICopyPasteService CopyPasteService
        {
            get { return LazyResolver<ICopyPasteService>.Service; }
        }

        public ITextToSpeechService TtsService
        {
            get { return LazyResolver<ITextToSpeechService>.Service; }
        }

        public IMediaService MediaService
        {
            get { return LazyResolver<IMediaService>.Service; }
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
        public ICommand SaveCommand { get; private set; }
        #endregion

        #region Properties
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
                        CurrentIndiagram = new Category();
                        CurrentIndiagram.CopyFrom(Indiagram.Indiagram);
                        IsCategory = true;
                    }
                    else
                    {
                        CurrentIndiagram = new Indiagram();
                        CurrentIndiagram.CopyFrom(Indiagram.Indiagram);
                    }
                    RefreshBrothers();
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
                if (!value && Indiagram != null && Indiagram.Indiagram.HasChildren)
                {
                    RaisePropertyChanged();
                    PopupService.DisplayPopup(LocalizationService.GetString("Collection_HasChildren", "Text"));
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

        /// <summary>
        /// Indiagram présent dans la même catégorie que l'Indiagram courant
        /// On en a besoin pour le classement dans la catégorie
        /// </summary>
        public List<Indiagram> Brothers
        {
            get { return _brothers; }
            set { SetProperty(ref _brothers, value); }
        }

        /// <summary>
        /// Dans la catégorie de l'Indiagram courant, Indiagram juste avant l'Indiagram courant
        /// </summary>
        public Indiagram BeforeIndiagram
        {
            get { return _beforeIndiagram; }
            set { SetProperty(ref _beforeIndiagram, value); }
        }
        #endregion

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
            RefreshBrothers();
        }

        /// <summary>
        /// Actualise la liste des Indiagram dans la catégorie de l'Indiagram courant
        /// </summary>
        protected void RefreshBrothers()
        {
            Category parent = CurrentIndiagram.Parent as Category;
            List<Indiagram> children;
            Indiagram defaultLastOne = new Indiagram
            {
                Id = -73,
                Text = LocalizationService.GetString("Collection_EndPosition", "Text")
            };
            if (parent == null)
            {
                children = CollectionStorageService.Collection.OrderBy(x => x.Position).ToList();
            }
            else
            {
                children = parent.Children.OrderBy(x => x.Position).ToList();
            }
            defaultLastOne.Position = children.Any() ? children.Last().Position + 1 : 1;
            children.Add(defaultLastOne);

            Indiagram selectedIndiagram = defaultLastOne;

            // remove current from the list and put the selected one at its place
            Indiagram current = children.FirstOrDefault(x => x.Id == CurrentIndiagram.Id);
            if (current != null)
            {
                int currentIndex = children.IndexOf(current);
                if (currentIndex + 1 < children.Count)
                {
                    selectedIndiagram = children[currentIndex + 1];
                }

                children.RemoveAt(currentIndex);
            }

            Brothers = children;
            BeforeIndiagram = selectedIndiagram;
        }

        /// <summary>
        /// Ouvre le dialogue de choix de la catégorie
        /// </summary>
        protected void SelectCategoryAction()
        {
            Indiagram excludedIndiagram = null;
            if (Indiagram != null)
            {
                excludedIndiagram = Indiagram.Indiagram;
            }
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_CHOOSE, new Dictionary<string, object>
			{
				{"ExcludedIndiagram", excludedIndiagram},
				{"SelectedCallback", (Action<Category>) OnCategorySelected}
			});
        }

        /// <summary>
        /// Callback pour le choix de la catégorie
        /// </summary>
        /// <param name="category">Catégorie choisi</param>
        private void OnCategorySelected(Category category)
        {
            if (!Data.Model.Indiagram.AreSameIndiagram(CurrentIndiagram.Parent, category))
            {
                CurrentIndiagram.Parent = category;
                RefreshBrothers();
            }
        }

        /// <summary>
        /// Active l'Indiagram courant
        /// </summary>
        protected void ActivateAction()
        {
            CurrentIndiagram.IsEnabled = true;
        }

        /// <summary>
        /// Désactive l'Indiagram courant
        /// </summary>
        protected void DesactivateAction()
        {
            CurrentIndiagram.IsEnabled = false;
        }

        /// <summary>
        /// Sauvegarde les modifications faites à l'Indiagram
        /// </summary>
        protected void SaveAction()
        {
            //todo : mettre un max char au nom sinon utilisateur
            if (string.IsNullOrWhiteSpace(CurrentIndiagram.Text))
            {
                PopupService.DisplayPopup(LocalizationService.GetString("Collection_MissingText", "Text"));
                return;
            }
            Indiagram savedIndiagram;
            if (EditMode)
            {
                //mode édition
                Indiagram original = Indiagram.Indiagram;
                Category parent = original.Parent as Category;
                if (parent == null)
                {
                    CollectionStorageService.Collection.Remove(original);
                }
                else
                {
                    parent.Children.Remove(original);
                }

                // edit indiagram in the storage
                if (original.IsCategory != IsCategory)
                {
                    savedIndiagram = IsCategory ? new Category() : new Indiagram();
                }
                else
                {
                    savedIndiagram = original;
                }
            }
            else
            {
                //create a new indiagram in the storage
                savedIndiagram = IsCategory ? new Category() : new Indiagram();
            }
            //sauvegarde l'indiagram
            savedIndiagram.CopyFrom(CurrentIndiagram);
            CollectionStorageService.Save(savedIndiagram);
            if (Indiagram != null)
            {
                Indiagram.Indiagram = savedIndiagram;
            }

            //met à jour le parent
            Category newParent = savedIndiagram.Parent as Category;
            ObservableCollection<Indiagram> collection;
            if (newParent == null)
            {
                collection = CollectionStorageService.Collection;
            }
            else
            {
                collection = newParent.Children;
            }

            if (BeforeIndiagram.Id < 0)
            {
                //put it at the end of the collection
                collection.Add(savedIndiagram);
            }
            else
            {
                int index = collection.IndexOf(BeforeIndiagram);
                collection.Insert(index, savedIndiagram);
            }

            //refresh collection position if needed
            int position = 1;
            foreach (Indiagram indiagram in collection)
            {
                if (indiagram.Position != position)
                {
                    indiagram.Position = position;
                    CollectionStorageService.Save(indiagram);
                }
                position++;
            }

            BackAction();
        }

        /// <summary>
        /// Ouvre le dialogue pour le choix de l'image
        /// </summary>
        protected void ImageChoiceAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_IMAGECHOICE, new Dictionary<string, object>
			{
				{"Indiagram", CurrentIndiagram}
			});
        }

        /// <summary>
        /// Ouvre le dialogue pour le choix du son
        /// </summary>
        protected void SoundChoiceAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_SOUNDCHOICE, new Dictionary<string, object>
			{
				{"Indiagram", CurrentIndiagram}
			});
        }

        /// <summary>
        /// Met l'Indiagram courant dans la catégorie Home
        /// </summary>
        protected void RootAction()
        {
            CurrentIndiagram.Parent = null;
            RefreshBrothers();
        }

        /// <summary>
        /// Supprime le son enregistré par l'utilisateur
        /// La TTS lira le nom de l'Indiagram lors de la prochaine lecture (sauf s'il y a une nouvelle modification entre temps)
        /// </summary>
        protected void ResetSoundAction()
        {
            CurrentIndiagram.SoundPath = null;
        }

        /// <summary>
        /// Lance la lecture de l'Indiagram
        /// </summary>
        protected void ListenAction()
        {
            if (string.IsNullOrWhiteSpace(CurrentIndiagram.Text) && !CurrentIndiagram.HasCustomSound)
            {
                PopupService.DisplayPopup(LocalizationService.GetString("Collection_MissingSound", "Text"));
            }
            else
            {
                TtsService.PlayIndiagram(CurrentIndiagram);
            }
        }

        /// <summary>
        /// Copie dans le buffer l'Indiagram courant
        /// </summary>
        protected void CopyAction()
        {
            CopyPasteService.Copy(CurrentIndiagram, IsCategory);
        }

        /// <summary>
        /// Colle le contenu du buffer à la place de l'Indiagram courant
        /// </summary>
        protected void PasteAction()
        {
            CurrentIndiagram = CopyPasteService.Paste();
            RefreshBrothers();
            IsCategory = CurrentIndiagram.IsCategory;
        }
    }
}