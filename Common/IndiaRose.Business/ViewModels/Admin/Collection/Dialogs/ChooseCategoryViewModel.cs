#region Usings 

using System;
using System.Collections.Generic;
using System.Linq;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    /// <summary>
    /// VueModèle de la page de choix de la catégorie parente 
    /// Page affichant tous les dialogues de la collection
    /// </summary>
	public class ChooseCategoryViewModel : AbstractBrowserViewModel
	{
		private Indiagram _excludedIndiagram;

		[NavigationParameter]
		public Indiagram ExcludedIndiagram
		{
			get { return _excludedIndiagram; }
			set { SetProperty(ref _excludedIndiagram, value); }
		}

		[NavigationParameter(Mode = NavigationParameterMode.Optional)]
		public Action<Category> SelectedCallback { get; set; }

		[NavigationParameter]
		public int DialogId { get; set; }

		protected override IEnumerable<Indiagram> FilterCollection(IEnumerable<Indiagram> input)
		{
			return input.Where(indiagram => indiagram.IsCategory && !Indiagram.AreSameIndiagram(indiagram, ExcludedIndiagram));
		}

		protected override void IndiagramSelectedAction(Indiagram indiagram)
		{
			base.IndiagramSelectedAction(indiagram);

			Category category = indiagram as Category;
			if (category == null)
			{
				return;
			}

			string dialogKey = Business.Dialogs.ADMIN_COLLECTION_SELECTCATEGORY_WITHOUTCHILDREN;
			if (category.Children.Any(x => x.IsCategory))
			{
				dialogKey = Business.Dialogs.ADMIN_COLLECTION_SELECTCATEGORY;
			}
			MessageDialogService.Show(dialogKey, new Dictionary<string, object>
			{
				{"Indiagram", indiagram},
				{"GoIntoCallback", (Action<Category>)GoIntoAction},
				{"SelectedCallback", (Action<Category>)OnCategorySelected}
			});	
		}

        /// <summary>
        /// A appeler lorsque la catégorie est sélectionné
        /// Ferme le dialogue courant et lance le callback passé en paramètre lors de la navigation vers la page
        /// </summary>
        /// <param name="category"></param>
		private void OnCategorySelected(Category category)
		{
			MessageDialogService.DismissDialog(DialogId);
			if (SelectedCallback != null)
			{
				SelectedCallback(category);
			}
		}

        /// <summary>
        /// Navigue vers la catégorie selectionné (seulement si elle contient d'autre catégorie)
        /// </summary>
        /// <param name="category">Catégorie vers laquelle navigué</param>
		private void GoIntoAction(Category category)
        {
            var trad = DependencyService.Container.Resolve<ILocalizationService>();
			if (!category.HasChildren)
			{
				var message = trad.GetString("Collection_CategoryEmpty", "Text");
				LazyResolver<IPopupService>.Service.DisplayPopup(message);
			}
			else
            {
                if (category.Children.All(x => !x.IsCategory))
                {
                    //s'il n'y a aucune catégorie on affiche un message
                    var message = trad.GetString("Collection_ChildrensNotCategory", "Text");
                    LazyResolver<IPopupService>.Service.DisplayPopup(message);
                }
                else
                {
                    PushCategory(category);
                }
			}
		}
	}
}