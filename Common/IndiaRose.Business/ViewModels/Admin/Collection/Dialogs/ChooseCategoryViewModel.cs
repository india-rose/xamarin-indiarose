#region Usings 

using System;
using System.Collections.Generic;
using System.Linq;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Messaging;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
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

		protected override IEnumerable<Indiagram> FilterCollection(IEnumerable<Indiagram> input)
		{
			return input.Where(indiagram => indiagram.IsCategory && !Indiagram.AreSameIndiagram(indiagram, ExcludedIndiagram));
		}

		protected override void IndiagramSelectedAction(Indiagram indiagram)
		{
			base.IndiagramSelectedAction(indiagram);

			
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_SELECTCATEGORY, new Dictionary<string, object>
			{
				{"Indiagram", indiagram},
				{"GoIntoCallback", (Action<Category>)GoIntoAction},
				{"SelectedCallback", (Action<Category>)OnCategorySelected}
			});
		}

		private void OnCategorySelected(Category category)
		{
			if (SelectedCallback != null)
			{
				SelectedCallback(category);
			}
			NavigationService.GoBack();
		}

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
	}
}