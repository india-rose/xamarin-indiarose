using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using IndiaRose.Business.ViewModels.Admin.Collection;
using IndiaRose.Business.ViewModels.Admin.Collection.Dialogs;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using Environment = Android.OS.Environment;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
	[BindingElement(Path = "WatchIndiagramCommand", TargetPath = "PositiveButtonEvent")]
	public partial class AddCollectionDialog : AlertDialogFragmentBase
	{
		public Indiagram _Indiagram;
		[Binding("Indiagram")]
		public Indiagram Indiagram
		{
			get { return _Indiagram; }
			set
			{
				if (SetProperty(ref _Indiagram, value))
				{
					RefreshView();
				}
			}
		}

		public AddCollectionDialog()
		{
			var trad = DependencyService.Container.Resolve<ILocalizationService>();
			Title = trad.GetString("whichActionQuestion", "Text");

			Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Back", "Text"));
			Buttons.Add(DialogsButton.Neutral, trad.GetString("goIntoText", "Text"));
			Buttons.Add(DialogsButton.Positive, trad.GetString("seeText", "Text"));

		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_AddCollectionDialog, container, false);
		}

		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminCollectionDialogsAddCollectionDialog;
		}

		public override void OnStart()
		{
			base.OnStart();
			Initialize();
		}

		private void RefreshView()
		{
			TextView parentTextView = RootView.FindViewById<TextView>(Resource.Id.m_indiagramCategory);
			Indiagram parent = Indiagram;
			if (parent != null)
				parentTextView.Text = parent.Text;
			else
			{
				var trad = DependencyService.Container.Resolve<ILocalizationService>();
				parentTextView.Text = trad.GetString("Root_Categ", "Text");
			}
		}
		private void Initialize()
		{
			ImageView imageView = RootView.FindViewById<ImageView>(Resource.Id.image);
			if (Indiagram != null && Indiagram.ImagePath != null)
				imageView.SetImageBitmap(
					Bitmap.CreateScaledBitmap(
						BitmapFactory.DecodeFile(Environment.ExternalStorageDirectory.Path + "/IndiaRose/image/" + Indiagram.ImagePath),
						imageView.Height, imageView.Width, true));
			else
				imageView.SetImageDrawable(new ColorDrawable(Color.Red));
		}
	}
}