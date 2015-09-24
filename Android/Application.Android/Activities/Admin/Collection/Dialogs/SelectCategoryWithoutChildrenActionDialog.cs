using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
	[BindingElement(Path = "SelectCommand", TargetPath = "PositiveButtonEvent")]
	public partial class SelectCategoryWithoutChildrenActionDialog : AlertDialogFragmentBase
	{
		public SelectCategoryWithoutChildrenActionDialog()
		{
			var trad = DependencyService.Container.Resolve<ILocalizationService>();
			Title = trad.GetString("ExploreCollection_TitleDialog", "Text");

			Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Cancel", "Text"));
			Buttons.Add(DialogsButton.Positive, trad.GetString("ExploreCollection_SelectCategory", "Text"));
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_SelectCategoryWithoutChildrenActionDialog, container, false);
		}
		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminCollectionDialogsSelectCategoryActionViewModel;
		}
	}
}