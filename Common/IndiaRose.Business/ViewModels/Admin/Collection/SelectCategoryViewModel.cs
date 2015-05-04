#region Usings 

using System.Collections.Generic;
using System.Linq;
using IndiaRose.Data.Model;
using Storm.Mvvm.Messaging;
using Storm.Mvvm.Navigation;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class SelectCategoryViewModel : AbstractBrowserViewModel
	{
		private Indiagram _indiagram;
		private Indiagram _excludedIndiagram;

		[NavigationParameter]
		public Indiagram Indiagram
		{
			get { return _indiagram; }
			set { SetProperty(ref _indiagram, value); }
		}
		[NavigationParameter]
		public Indiagram ExcludedIndiagram
		{
			get { return _excludedIndiagram; }
			set { SetProperty(ref _excludedIndiagram, value); }
		}

		protected override IEnumerable<Indiagram> FilterCollection(IEnumerable<Indiagram> input)
		{
			return input.Where(indiagram => indiagram.IsCategory && !Indiagram.AreSameIndiagram(indiagram, ExcludedIndiagram));
		}

		protected override void IndiagramSelectedAction(Indiagram indiagram)
		{
			base.IndiagramSelectedAction(indiagram);

			Messenger.Register<Category>(Messages.SELECT_CATEGORY_GOINTO_CATEGORY, this, PushCategory);
			Messenger.Register<Category>(Messages.SELECT_CATEGORY_SELECTED_CATEGORY, this, cat =>
			{
				Indiagram.Parent = cat;
				NavigationService.GoBack();
			});
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_SELECTCATEGORY, new Dictionary<string, object>
			{
				{"Indiagram", indiagram}
			}, () =>
			{
				Messenger.Unregister(this, Messages.SELECT_CATEGORY_GOINTO_CATEGORY);
				Messenger.Unregister(this, Messages.SELECT_CATEGORY_SELECTED_CATEGORY);
			});
		}
	}
}