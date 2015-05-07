using Storm.Mvvm;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels
{
	public class ImportingCollectionViewModel : ViewModelBase
	{
		private string _message;

		public string Message
		{
			get { return _message; }
			set { SetProperty(ref _message, value); }
		}

		[NavigationParameter]
		public string MessageUid
		{
			set { Message = LocalizationService.GetString(value, "Text"); }
		}

		protected ILocalizationService LocalizationService
		{
			get { return LazyResolver<ILocalizationService>.Service; }
		}

	}
}
