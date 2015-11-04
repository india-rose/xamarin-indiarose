using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndiaRose.WebAPI.Sdk.Models;
using IndiaRose.WebAPI.Sdk.Results;
using PCLCrypto;
using WebAPI.Common.Responses;

namespace IndiaRose.Business.ViewModels.Admin.Synchronization
{
	public partial class SynchronizationViewModel
	{
		private string _userLogin;
		private string _userPassword;
		private DeviceResponse _selectedDevice;
		private List<DeviceResponse> _devices; 

		private async Task<bool> ConnectUserAsync(string login, string password)
		{
			UserStatusCode status = await ApiService.LoginUserAsync(login, password);
			if (status != UserStatusCode.Ok)
			{
				return false;
			}

			_userLogin = login;
			_userPassword = password;

			return true;
		}

		private async Task<bool> CacheDeviceList()
		{
			ApiResult<DeviceStatusCode, List<DeviceResponse>> result = await ApiService.ListDevicesAsync(GetUserInfo());

			if (result.Status != DeviceStatusCode.Ok)
			{
				return false;
			}


			_devices = result.Content;
			return true;
		}

		private UserInfo GetUserInfo()
		{
			return new UserInfo()
			{
				Login = _userLogin,
				Password = _userPassword,
			};
		}

		private string HashPassword(string passwd)
		{
			byte[] data = Encoding.UTF8.GetBytes(passwd);
			IHashAlgorithmProvider hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
			byte[] hash = hasher.HashData(data);

			return string.Join("", hash.Select(x => x.ToString("X2")));
		}

		public async Task<bool> SelectDeviceAsync(string deviceName)
		{
			if (string.IsNullOrEmpty(deviceName))
			{
				return false;
			}

			if (!await CacheDeviceList())
			{
				return false;
			}
			_selectedDevice = _devices.FirstOrDefault(x => x.Name == SynchronizationSettingsService.DeviceName);
			return _selectedDevice != null;
		}
	}
}
