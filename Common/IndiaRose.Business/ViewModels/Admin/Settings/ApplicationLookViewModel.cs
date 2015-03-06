#region Usings

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings.Dialogs;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
	public class ApplicationLookViewModel : AbstractSettingsViewModel
	{
		#region Properties

		private int _currentSize;
		private ColorContainer _topColor;
		private ColorContainer _bottomColor;

		public ColorContainer BottomColor
		{
			get { return _bottomColor; }
			set { SetProperty(ref _bottomColor, value); }
		}
		public ColorContainer TopColor
		{
			get { return _topColor; }
			set { SetProperty(ref _topColor, value); }
		}
		public int CurrentSize
		{
			get { return _currentSize; }
			set { SetProperty(ref _currentSize, value); }
		}

		public ObservableCollection<int> ZoneHeightCollection { get; private set; }

		#endregion

		#region Services

		private IMessageDialogService _messageDialogService;
		private IScreenService _screenService;

		public IMessageDialogService MessageDialogService
		{
			get { return _messageDialogService ?? (_messageDialogService = Container.Resolve<IMessageDialogService>()); }
		}

		public IScreenService ScreenService
		{
			get { return _screenService ?? (_screenService = Container.Resolve<IScreenService>()); }
		}

		#endregion

		#region Commands

		public ICommand TopAreaColorCommand { get; private set; }
		public ICommand BottomAreaColorCommand { get; private set; }

		#endregion

		public ApplicationLookViewModel(IContainer container)
			: base(container)
		{
			TopAreaColorCommand = new DelegateCommand(TopAreaColorAction);
			BottomAreaColorCommand = new DelegateCommand(BottomAreaColorAction);

			ZoneHeightCollection = new ObservableCollection<int>();
			InitZoneHeightsArray();
		    CurrentSize = SettingsService.SelectionAreaHeight;
			BottomColor = new ColorContainer
			{
				Color = SettingsService.BottomBackgroundColor
			};
			TopColor = new ColorContainer
			{
				Color = SettingsService.TopBackgroundColor
			};
		}

		private void BottomAreaColorAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>
			{
				{ColorPickerViewModel.COLOR_CONTAINER_PARAMETER, BottomColor}
			});
		}

		private void TopAreaColorAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>
			{
				{ColorPickerViewModel.COLOR_CONTAINER_PARAMETER, TopColor}
			});
		}

		private void InitZoneHeightsArray()
		{
			// clear the collection before filling it
            ZoneHeightCollection.Clear();

			// calculate the minimal size of the top area
			// : indiagram size + 20% 
			int minHeight = (int) ((SettingsService.IndiagramDisplaySize*1.2)/ScreenService.Height*100);
			
			// calculate the maximal size of the top area
			// : 95% of the screen minus the minimal size of the bottom area.
			int maxHeight = 95 - minHeight;

			for (int i = minHeight; i <= maxHeight; ++i)
			{
                ZoneHeightCollection.Add(i);
			}
		}

		protected override void SaveAction()
		{
			SettingsService.SelectionAreaHeight = CurrentSize;
			SettingsService.TopBackgroundColor = TopColor.Color;
			SettingsService.BottomBackgroundColor = BottomColor.Color;
			base.SaveAction();
		}
	}
}