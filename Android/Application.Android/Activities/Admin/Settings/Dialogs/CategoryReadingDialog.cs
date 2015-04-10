#region Usings

using System;
using System.Globalization;
using Android.Views;
using Android.Widget;
using Java.Util;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings.Dialogs
{
	[BindingElement(Path = "SaveCommand", TargetPath = "PositiveButtonEvent")]
	public partial class CategoryReadingDialog : AlertDialogFragmentBase
	{
		public CategoryReadingDialog()
		{
		    var trad = DependencyService.Container.Resolve<ILocalizationService>();

		            Title = trad.GetString("Button_Back","Text");
		            Buttons.Add(DialogsButton.Positive, "Ok");
		            Buttons.Add(DialogsButton.Negative, "Annuler");
		}

	    protected override ViewModelBase CreateViewModel()
	    {
			return Container.Locator.AdminSettingsDialogsCategoryReadingViewModel;
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			//throw new NotImplementedException();
            return inflater.Inflate(Resource.Layout.Admin_Settings_Dialogs_CategoryReading, container, false);
		}
	}
}
