using Android.App;
using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
		
	[BindingElement(Path = "WatchIndiagramCommand", TargetPath = "PositiveButtonEvent")]
	public partial class DeleteIndiagramDialog : AlertDialogFragmentBase
	{
		public DeleteIndiagramDialog()
		{
			var trad = DependencyService.Container.Resolve<ILocalizationService>();
			Title = trad.GetString("whichActionQuestion", "Text");
			Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Back", "Text"));
			Buttons.Add(DialogsButton.Neutral, trad.GetString("remove", "Text"));
			Buttons.Add(DialogsButton.Positive, trad.GetString("seeText", "Text"));
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_DeleteIndiagramDialog, container, false);
		}
		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminCollectionDialogsDeleteIndiagramViewModel;
		}
	}
}
