#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    /// <summary>
    /// VueModèle de la première page de la partie collection (affichage de la collection)
    /// </summary>
    public class CollectionManagementViewModel : AbstractBrowserViewModel
    {
        public ICommand AddCommand { get; private set; }

        public IPopupService PopupService
        {
            get { return LazyResolver<IPopupService>.Service; }
        }

        public CollectionManagementViewModel()
        {
            AddCommand = new DelegateCommand(AddAction);
        }

        #region Actions

        /// <summary>
        /// Ouvre la page d'ajout d'Indiagram
        /// </summary>
        private void AddAction()
        {
            // todo parameter can be removed when the framework has been updated
            NavigationService.Navigate(Views.ADMIN_COLLECTION_ADDINDIAGRAM);
        }

        protected override void IndiagramSelectedAction(Indiagram indiagram)
        {
            base.IndiagramSelectedAction(indiagram);

            if (indiagram.IsCategory)
            {
                string dialogKey = indiagram.HasChildren ? Business.Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_CATEGORY : Business.Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_CATEGORY_WITHOUTCHILDREN;
                MessageDialogService.Show(dialogKey, new Dictionary<string, object>
				{
					{"Indiagram", indiagram},
					{"GoIntoCallback", (Action<Category>)GoIntoAction }
				});
            }
            else
            {
                MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_INDIAGRAM, new Dictionary<string, object>
				{
					{"Indiagram", indiagram}
				});
            }
        }

        /// <summary>
        /// Navigue vers la catégorie
        /// Si la catégorie n'a pas d'Indiagram, affiche un message
        /// </summary>
        /// <param name="category"></param>
        private void GoIntoAction(Category category)
        {
            if (!category.HasChildren)
            {
                var trad = DependencyService.Container.Resolve<ILocalizationService>();
                var message = trad.GetString("Collection_CategoryEmpty", "Text");
                LazyResolver<IPopupService>.Service.DisplayPopup(message);
            }
            else
            {
                PushCategory(category);
            }
        }
        #endregion
    }
}