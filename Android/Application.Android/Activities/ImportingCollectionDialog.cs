using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities
{
	public partial class ImportingCollectionDialog : AlertDialogFragmentBase
	{
		public ImportingCollectionDialog()
		{
			Title = LazyResolver<ILocalizationService>.Service.GetString("ImportCollection_DialogTitle", "Text");
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			Cancelable = false;
			return inflater.Inflate(Resource.Layout.ImportingCollectionDialog, container, false);
		}

		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.ImportingCollectionViewModel;
		}
	}
}