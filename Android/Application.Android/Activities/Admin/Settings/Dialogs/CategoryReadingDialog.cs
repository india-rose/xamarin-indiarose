#region Usings

using System;
using System.Globalization;
using Android.Views;
using Java.Util;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings.Dialogs
{
	[BindingElement(Path = "SaveCommand", TargetPath = "PositiveButtonEvent")]
	public partial class CategoryReadingDialog : AlertDialogFragmentBase
	{
		public CategoryReadingDialog()
		{
		    var ci = CultureInfo.InstalledUICulture;
		        if (ci.TwoLetterISOLanguageName=="fr")
		        {
		            Title = "Lire le son associé à la catégorie lors de la sélection ?";
		            Buttons.Add(DialogsButton.Positive, "Ok");
		            Buttons.Add(DialogsButton.Negative, "Annuler");
		        }
		        else
		        {
		            Title = "Read the associated sound associated with the category during the selection ?";
		            Buttons.Add(DialogsButton.Positive, "Ok");
		            Buttons.Add(DialogsButton.Negative, "Back");
		        }
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
