using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IndiaRose.WebAPI.Sdk.Results;
using Storm.Mvvm.Commands;

namespace IndiaRose.Business.ViewModels.Admin.Synchronization
{
	public partial class SynchronizationViewModel
	{
		private List<string> _inputDeviceNames;
		private string _inputSelectedDeviceName;
		private string _inputNewDeviceName;

		public List<string> InputDeviceNames
		{
			get { return _inputDeviceNames; }
			set { SetProperty(ref _inputDeviceNames, value); }
		}

		public string InputSelectedDeviceName
		{
			get { return _inputSelectedDeviceName; }
			set { SetProperty(ref _inputSelectedDeviceName, value); }
		}

		public string InputNewDeviceName
		{
			get { return _inputNewDeviceName; }
			set { SetProperty(ref _inputNewDeviceName, value); }
		}

		public ICommand SelectDeviceCommand { get; private set; }
		public ICommand CreateDeviceCommand { get; private set; }

		public void DeviceChooseInit()
		{
			SelectDeviceCommand = new DelegateCommand(SelectDeviceAction);
			CreateDeviceCommand = new DelegateCommand(CreateDeviceAction);

			InputDeviceNames = null;
			InputSelectedDeviceName = null;
		}

		partial void OnNavigatedToDeviceChoose()
		{
			InputDeviceNames = new List<string>();
			InputSelectedDeviceName = null;

			CacheDeviceList().ContinueWith(t =>
			{
				if (t.Result)
				{
					DispatcherService.InvokeOnUIThread(() =>
					{
						if (_devices != null)
						{
							InputDeviceNames = _devices.Select(x => x.Name).ToList();
						}
						InputSelectedDeviceName = InputDeviceNames.FirstOrDefault();
					});
				}
				else
				{
					DispatcherService.InvokeOnUIThread(() => SetError(LocalizationService.GetString("DeviceChoose_CannotRetrieveList", "Text")));
				}
			});
		}

		private async void SelectDeviceAction()
		{
			if (string.IsNullOrWhiteSpace(InputSelectedDeviceName) || !await SelectDeviceAsync(InputSelectedDeviceName))
			{
				SetError(LocalizationService.GetString("DeviceChoose_NoDeviceSelected", "Text"));
			}

			TransitionToState(SynchronizationPageState.SummaryPage);
		}

		private async void CreateDeviceAction()
		{
			if (string.IsNullOrWhiteSpace(InputNewDeviceName))
			{
				SetError(LocalizationService.GetString("DeviceChoose_DeviceNameEmpty", "Text"));
				return;
			}

			DeviceStatusCode resultCode = await ApiService.CreateDeviceAsync(GetUserInfo(), InputNewDeviceName);
			if (resultCode != DeviceStatusCode.Ok)
			{
				await SelectDeviceAsync(InputNewDeviceName);
				TransitionToState(SynchronizationPageState.SummaryPage);
			}
			else
			{
				string errorUid;
				if (resultCode == DeviceStatusCode.DeviceAlreadyExists)
				{
					errorUid = "DeviceAlreadyExists";
				}
				else
				{
					errorUid = "UnableToCreateDevice";
				}
				SetError(LocalizationService.GetString(string.Format("DeviceChoose_{0}", errorUid), "Text"));
			}
		}
	}
}
